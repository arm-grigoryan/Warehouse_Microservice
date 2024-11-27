using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;

namespace Warehouse.ApplicationCore.Categories.Create;

public record CreateCategoryCommand(
    string Name,
    string Description) : IRequest<CategoryDto>;
