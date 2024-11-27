using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interfaces.Persistence;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence.Data.Repositories;

internal class OrdersRepository(
    IOptions<WarehouseDBSettings> settings,
    WarehouseDbContext context,
    IMapper mapper) : IOrdersRepository
{
    private readonly IMongoCollection<OrderDb> _collection = context.Database.GetCollection<OrderDb>(settings.Value.OrdersCollectionName);

    public async Task<Order> AddAsync(Order product, CancellationToken cToken = default)
    {
        var dbEntity = mapper.Map<OrderDb>(product);
        await _collection.InsertOneAsync(dbEntity, cancellationToken: cToken);
        return mapper.Map<Order>(dbEntity);
    }

    public async Task<List<Order>> GetAllAsync(CancellationToken cToken = default)
    {
        var entities = await _collection.Find(_ => true).ToListAsync(cToken);
        return mapper.Map<List<Order>>(entities);
    }

    public async Task UpdateStatusAsync(string orderId, OrderStatus status, CancellationToken cToken = default)
    {
        var result = await _collection.UpdateOneAsync(x => x.Id == orderId, Builders<OrderDb>.Update.Set(p => p.Status, status), cancellationToken: cToken);

        if (!result.IsAcknowledged)
        {
            throw new WarehouseException("Update operation was not acknowledged by the server.");
        }

        if (result.MatchedCount == 0)
        {
            throw new WarehouseException("No order matched the filter.", HttpStatusCode.NotFound);
        }
    }
}
