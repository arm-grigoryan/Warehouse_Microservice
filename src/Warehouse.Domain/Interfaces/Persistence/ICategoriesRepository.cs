using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interfaces.Persistence;

public interface ICategoriesRepository
{
    Task<Category> AddAsync(Category product, CancellationToken cToken = default);
    Task<Category> UpdateAsync(Category product, CancellationToken cToken = default);
    Task<Category> GetByIdAsync(string id, CancellationToken cToken = default);
    Task<List<Category>> GetAllAsync(CancellationToken cToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cToken = default);
}
