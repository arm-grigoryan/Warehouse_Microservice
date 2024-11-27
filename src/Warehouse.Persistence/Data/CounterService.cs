using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Warehouse.Persistence.Data.Entities;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence.Data;

internal class CounterService(IOptions<WarehouseDBSettings> settings, WarehouseDbContext context)
{
    private readonly IMongoCollection<CounterDb> _counterCollection = context.Database.GetCollection<CounterDb>(settings.Value.CounterCollectionName);

    public async Task<int> GetNextSequenceValueAsync(string counterName, CancellationToken cToken = default)
    {
        var filter = Builders<CounterDb>.Filter.Eq(c => c.Id, counterName);
        var update = Builders<CounterDb>.Update.Inc(c => c.SequenceValue, 1);

        var options = new FindOneAndUpdateOptions<CounterDb>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var counter = await _counterCollection.FindOneAndUpdateAsync(filter, update, options, cToken);
        return counter.SequenceValue;
    }
}
