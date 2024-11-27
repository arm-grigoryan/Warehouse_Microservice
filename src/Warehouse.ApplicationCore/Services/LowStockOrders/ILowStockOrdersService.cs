using Warehouse.Domain.Entities;

namespace Warehouse.ApplicationCore.Services.LowStockOrders;

public interface ILowStockOrdersService
{
    Task<Order> ReserveWithManualApprovalAsync(Product product, int quantity, CancellationToken cToken = default);
}
