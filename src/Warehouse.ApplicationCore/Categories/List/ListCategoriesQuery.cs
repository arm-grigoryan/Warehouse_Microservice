using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;

namespace Warehouse.ApplicationCore.Categories.List;

public record ListCategoriesQuery : IRequest<List<CategoryDto>>;
