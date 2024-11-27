using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces.Persistence;

public interface IProductsRepository
{
    Task<Product> AddAsync(Product product, CancellationToken cToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cToken = default);
    Task UpdateStockAsync(int productId, int stock, CancellationToken cToken = default);
    Task<Product> GetByIdAsync(int id, CancellationToken cToken = default);
}
