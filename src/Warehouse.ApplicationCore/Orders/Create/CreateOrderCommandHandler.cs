using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.ApplicationCore.Services.AvailableStockOrders;
using Warehouse.ApplicationCore.Services.LowStockOrders;
using Warehouse.ApplicationCore.Services.PendingOrders;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Orders.Create;

public class CreateOrderCommandHandler(
    IProductsRepository productsRepository,
    IAvailableStockOrdersService availableStockOrdersService,
    ILowStockOrdersService lowStockOrdersService,
    IPendingOrdersService pendingOrdersService,
    IConfiguration configuration,
    IMapper mapper) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var product = await productsRepository.GetByIdAsync(request.ProductId, cancellationToken)
                            ?? throw new WarehouseException($"Product with ID : {request.ProductId} not found.", HttpStatusCode.NotFound);

        var stockStatus = product.Status;

        var rejectOutOfStockRequests = configuration.GetValue<bool>("RejectOutOfStockRequests");

        var createdOrder = await (stockStatus switch
        {
            StockStatus.Available =>
                availableStockOrdersService.ReserveDirectlyAsync(product, request.Quantity, cancellationToken),
            StockStatus.LowStock =>
                lowStockOrdersService.ReserveWithManualApprovalAsync(product, request.Quantity, cancellationToken),
            StockStatus.OutOfStock when !rejectOutOfStockRequests =>
                pendingOrdersService.CreateOrderAsync(product.Id, request.Quantity, cancellationToken).ContinueWith(t => (Order)t.Result, cancellationToken),
            StockStatus.OutOfStock when rejectOutOfStockRequests =>
                throw new WarehouseException("The selected product is currently out of stock.", HttpStatusCode.BadRequest),
            _ =>
                throw new WarehouseException($"Unhandled stock status: {stockStatus}", HttpStatusCode.BadRequest)
        });

        return mapper.Map<OrderDto>(createdOrder);
    }
}
