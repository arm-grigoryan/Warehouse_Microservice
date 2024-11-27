using AutoMapper;
using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Categories.Create;

public class CreateCategoryCommandHandler(
    ICategoriesRepository repository,
    IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category { Name = request.Name, Description = request.Description };
        var created = await repository.AddAsync(category, cancellationToken);
        return mapper.Map<CategoryDto>(created);
    }
}
