using MediatR;
using Warehouse.ApplicationCore.Categories.DTOs;

namespace Warehouse.ApplicationCore.Categories.Get;

public record GetCategoryQuery(string CategoryId) : IRequest<CategoryDto>;
