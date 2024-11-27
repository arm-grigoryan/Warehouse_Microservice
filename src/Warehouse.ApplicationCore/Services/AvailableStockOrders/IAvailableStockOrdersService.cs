using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Services.AvailableStockOrders;

public interface IAvailableStockOrdersService
{
    Task<Order> ReserveDirectlyAsync(Product product, int quantity, CancellationToken cToken = default);
}
