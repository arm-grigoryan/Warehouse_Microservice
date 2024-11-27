using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces.Persistence;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence.Data.Repositories;

internal class PendingOrdersRepository(
    IOptions<WarehouseDBSettings> settings,
    WarehouseDbContext context,
    IMapper mapper) : IPendingOrdersRepository
{
    private readonly IMongoCollection<PendingOrderDb> _collection = context.Database.GetCollection<PendingOrderDb>(settings.Value.PendingOrdersCollectionName);

    public async Task<PendingOrder> AddAsync(PendingOrder pendingOrder, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<PendingOrderDb>(pendingOrder);
        await _collection.InsertOneAsync(dbEntity, cancellationToken: cToken);
        return mapper.Map<PendingOrder>(dbEntity);
    }

    public async Task<List<PendingOrder>> GetAllByProductIdAsync(int productId, CancellationToken cToken = default)
    {
        var entities = await _collection.Find(x => x.ProductId == productId).ToListAsync(cToken);
        return mapper.Map<List<PendingOrder>>(entities);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cToken = default)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id, cToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
