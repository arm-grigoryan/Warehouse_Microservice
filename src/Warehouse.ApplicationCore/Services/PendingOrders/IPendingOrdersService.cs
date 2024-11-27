using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Services.PendingOrders;

public interface IPendingOrdersService
{
    Task<PendingOrder> CreateOrderAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    Task FulfillOrdersAsync(int productId, CancellationToken cToken = default);
}
