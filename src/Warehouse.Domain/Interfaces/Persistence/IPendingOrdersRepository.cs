using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces.Persistence;

public interface IPendingOrdersRepository
{
    Task<PendingOrder> AddAsync(PendingOrder pendingOrder, CancellationToken cToken = default);
    Task<List<PendingOrder>> GetAllByProductIdAsync(int productId, CancellationToken cToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cToken = default);
}
