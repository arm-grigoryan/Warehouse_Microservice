using MongoDB.Driver;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.Persistence.Data.Repositories;

internal class UnitOfWork(WarehouseDbContext context) : IUnitOfWork
{
    private IClientSessionHandle _session;

    public async Task BeginTransactionAsync(CancellationToken cToken = default)
    {
        if (_session != null)
        {
            throw new WarehouseException("A transaction is already active.");
        }

        _session = await context.Client.StartSessionAsync(cancellationToken: cToken);
        _session.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_session == null)
        {
            throw new WarehouseException("No active transaction to commit.");
        }

        await _session.CommitTransactionAsync(cancellationToken);
        _session.Dispose();
        _session = null;
    }

    public async Task AbortTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_session == null)
        {
            throw new WarehouseException("No active transaction to abort.");
        }

        await _session.AbortTransactionAsync(cancellationToken);
        _session.Dispose();
        _session = null;
    }
}
