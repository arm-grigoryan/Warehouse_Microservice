using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Products.Create;

public class CreateProductCommandHandler(
    IProductsRepository repository,
    IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            Stock = request.Stock,
            LowStockThreshold = request.LowStockThreshold,
            OutOfStockThreshold = request.OutOfStockThreshold,
            CreatedDate = DateTime.UtcNow
        };

        var createdProduct = await repository.AddAsync(product, cancellationToken);
        return mapper.Map<ProductDto>(createdProduct);
    }
}
