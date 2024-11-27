using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;

namespace Warehouse.ApplicationCore.Products.Create;

public record CreateProductCommand(
    string Name,
    string CategoryId,
    int Stock,
    int LowStockThreshold,
    int OutOfStockThreshold) : IRequest<ProductDto>;