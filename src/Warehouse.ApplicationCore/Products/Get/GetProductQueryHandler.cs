using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Products.DTOs;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Products.Get;

public class GetProductQueryHandler(
    IProductsRepository repository,
    IMapper mapper) : IRequestHandler<GetProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);
        return mapper.Map<ProductDto>(product);
    }
}
