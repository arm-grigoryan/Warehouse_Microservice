using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;

namespace Warehouse.Domain.Interfaces.Persistence;

public interface IOrdersRepository
{
    Task<Order> AddAsync(Order product, CancellationToken cToken = default);
    Task<List<Order>> GetAllAsync(CancellationToken cToken = default);
    Task UpdateStatusAsync(string orderId, OrderStatus status, CancellationToken cToken = default);
}
