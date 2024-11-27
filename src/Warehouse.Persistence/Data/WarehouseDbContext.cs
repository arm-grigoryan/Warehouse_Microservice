using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence.Data;

public class WarehouseDbContext
{
    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public WarehouseDbContext(IOptions<WarehouseDBSettings> settings)
    {
        Client = new MongoClient(settings.Value.ConnectionString);
        Database = Client.GetDatabase(settings.Value.DatabaseName);
    }
}