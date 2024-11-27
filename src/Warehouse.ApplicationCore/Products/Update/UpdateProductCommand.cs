using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;

namespace Warehouse.ApplicationCore.Products.Update;

public record UpdateProductCommand(
    int Id,
    string Name,
    string CategoryId,
    int Stock,
    int LowStockThreshold,
    int OutOfStockThreshold,
    DateTime CreatedDate) : IRequest<ProductDto>;
