using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence.Data.Repositories;

internal class CategoriesRepository(
    IOptions<WarehouseDBSettings> settings,
    WarehouseDbContext context,
    IMapper mapper) : ICategoriesRepository
{
    private readonly IMongoCollection<CategoryDb> _collection = context.Database.GetCollection<CategoryDb>(settings.Value.CategoriesCollectionName);

    public async Task<Category> AddAsync(Category product, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<CategoryDb>(product);
        await _collection.InsertOneAsync(dbEntity, cancellationToken: cToken);
        return mapper.Map<Category>(dbEntity);
    }

    public async Task<Category> UpdateAsync(Category product, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<CategoryDb>(product);
        var result = await _collection.ReplaceOneAsync(x => x.Id == dbEntity.Id, dbEntity, cancellationToken: cToken);

        if (!result.IsAcknowledged)
        {
            throw new WarehouseException("Replace operation was not acknowledged by the server.");
        }

        if (result.MatchedCount == 0)
        {
            throw new WarehouseException("No document matched the filter.", HttpStatusCode.NotFound);
        }

        return mapper.Map<Category>(dbEntity);
    }

    public async Task<Category> GetByIdAsync(string id, CancellationToken cToken = default)
    {
        var dbEntity = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cToken);
        return mapper.Map<Category>(dbEntity);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cToken = default)
    {
        var entities = await _collection.Find(_ => true).ToListAsync(cToken);
        return mapper.Map<List<Category>>(entities);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cToken = default)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id, cToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
