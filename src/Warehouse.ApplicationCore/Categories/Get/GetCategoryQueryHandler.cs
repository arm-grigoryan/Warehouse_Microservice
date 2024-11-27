using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Categories.Get;

public class GetCategoryQueryHandler(
    ICategoriesRepository repository,
    IMapper mapper) : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.CategoryId, cancellationToken);
        return mapper.Map<CategoryDto>(category);
    }
}
