namespace Warehouse.Domain.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task AbortTransactionAsync(CancellationToken cancellationToken = default);
}
