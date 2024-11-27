using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Categories.Update;

public class UpdateCategoryCommandHandler(
    ICategoriesRepository repository,
    IMapper mapper) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        };
        var updated = await repository.UpdateAsync(category, cancellationToken);
        return mapper.Map<CategoryDto>(updated);
    }
}
