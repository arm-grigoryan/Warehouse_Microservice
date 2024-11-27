using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Categories.List;

public class ListCategoriesQueryHandler(
    ICategoriesRepository repository,
    IMapper mapper) : IRequestHandler<ListCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<List<CategoryDto>>(categories);
    }
}
