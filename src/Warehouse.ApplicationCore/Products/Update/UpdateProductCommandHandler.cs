using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;
using Warehouse.ApplicationCore.Services.PendingOrders;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Products.Update;

public class UpdateProductCommandHandler(
    IProductsRepository repository,
    IPendingOrdersService pendingOrdersService,
    IMapper mapper) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            CategoryId = request.CategoryId,
            Stock = request.Stock,
            LowStockThreshold = request.LowStockThreshold,
            OutOfStockThreshold = request.OutOfStockThreshold,
            CreatedDate = request.CreatedDate,
            UpdatedDate = DateTime.UtcNow
        };

        var updatedProduct = await repository.UpdateAsync(product, cancellationToken);

        // FulFill pending orders
        await pendingOrdersService.FulfillOrdersAsync(request.Id, cancellationToken);

        return mapper.Map<ProductDto>(updatedProduct);
    }
}
