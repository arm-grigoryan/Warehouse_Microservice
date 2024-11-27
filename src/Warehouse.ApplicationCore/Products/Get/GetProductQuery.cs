using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;

namespace Warehouse.ApplicationCore.Products.Get;

public record GetProductQuery(int ProductId) : IRequest<ProductDto>;
