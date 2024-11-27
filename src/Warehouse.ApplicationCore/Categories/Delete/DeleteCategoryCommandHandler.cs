using MediatR;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.ApplicationCore.Categories.Delete;

public class DeleteCategoryCommandHandler(
    ICategoriesRepository repository) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(request.Id, cancellationToken);
    }
}
