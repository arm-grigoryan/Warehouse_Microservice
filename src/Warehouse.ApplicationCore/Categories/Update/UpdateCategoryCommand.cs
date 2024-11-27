using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;

namespace Warehouse.ApplicationCore.Categories.Update;

public record UpdateCategoryCommand(
    string Id,
    string Name,
    string Description) : IRequest<CategoryDto>;
