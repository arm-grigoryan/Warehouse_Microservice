using System.Net;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Infrastructure;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Services.LowStockOrders;

internal class LowStockOrdersService(
    IOrdersRepository ordersRepository,
    IMessagePublisher messagePublisher) : ILowStockOrdersService
{
    public async Task<Order> ReserveWithManualApprovalAsync(Product product, int quantity, CancellationToken cToken = default)
    {
        var newStockAmount = product.Stock - quantity;
        if (newStockAmount < 0)
        {
            throw new WarehouseException("Insufficient stock to fulfill the order.", HttpStatusCode.BadRequest);
        }

        // Create and save the order
        var order = new Order
        {
            ProductId = product.Id,
            Quantity = quantity,
            Status = OrderStatus.UnderReview,
            OrderDate = DateTime.UtcNow
        };
        var createdOrder = await ordersRepository.AddAsync(order, cToken);

        // Publish order placed event for approval
        var orderPlaced = new OrderPlaced
        {
            CorrelationId = Guid.NewGuid(),
            OrderId = createdOrder.Id,
            ProductId = createdOrder.ProductId,
            CurrentProductStock = product.Stock,
            Quantity = createdOrder.Quantity
        };
        await messagePublisher.Publish(orderPlaced, cToken);

        return createdOrder;
    }
}
