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

internal class ProductsRepository(
    CounterService counterService,
    IOptions<WarehouseDBSettings> settings,
    WarehouseDbContext context,
    IMapper mapper) : IProductsRepository
{
    private readonly IMongoCollection<ProductDb> _collection = context.Database.GetCollection<ProductDb>(settings.Value.ProductsCollectionName);

    public async Task<Product> AddAsync(Product product, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<ProductDb>(product);
        dbEntity.Id = await counterService.GetNextSequenceValueAsync(settings.Value.ProductsCollectionName, cToken);
        await _collection.InsertOneAsync(dbEntity, cancellationToken: cToken);
        return mapper.Map<Product>(dbEntity);
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<ProductDb>(product);
        var result = await _collection.ReplaceOneAsync(x => x.Id == dbEntity.Id, dbEntity, cancellationToken: cToken);

        if (!result.IsAcknowledged)
        {
            throw new WarehouseException("Replace operation was not acknowledged by the server.");
        }

        if (result.MatchedCount == 0)
        {
            throw new WarehouseException("No document matched the filter.", HttpStatusCode.NotFound);
        }

        return mapper.Map<Product>(dbEntity);
    }

    public async Task UpdateStockAsync(int productId, int stock, CancellationToken cToken = default)
    {
        var result = await _collection.UpdateOneAsync(x => x.Id == productId, Builders<ProductDb>.Update.Set(p => p.Stock, stock), cancellationToken: cToken);

        if (!result.IsAcknowledged)
        {
            throw new WarehouseException("Update operation was not acknowledged by the server.");
        }

        if (result.MatchedCount == 0)
        {
            throw new WarehouseException("No document matched the filter.", HttpStatusCode.NotFound);
        }
    }

    public async Task<Product> GetByIdAsync(int id, CancellationToken cToken = default)
    {
        var dbEntity = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cToken);
        return mapper.Map<Product>(dbEntity);
    }
}
