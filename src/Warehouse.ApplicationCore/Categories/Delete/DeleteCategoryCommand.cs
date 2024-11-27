using MediatR;

namespace Warehouse.ApplicationCore.Categories.Delete;

public record DeleteCategoryCommand(string Id) : IRequest<bool>;