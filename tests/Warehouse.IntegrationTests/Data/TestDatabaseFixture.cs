using Microsoft.Extensions.Options;
using Mongo2Go;
using Warehouse.Persistence.Data;
using Warehouse.Persistence.Data.Configurations;
using Warehouse.Persistence.Settings;

namespace Warehouse.IntegrationTests.Data;

public class TestDatabaseFixture : IDisposable
{
    private readonly MongoDbRunner _runner;

    public IOptions<WarehouseDBSettings> Settings { get; }
    public WarehouseDbContext WarehouseDbContext { get; }

    public TestDatabaseFixture()
    {
        _runner = MongoDbRunner.Start();

        Settings = Options.Create(new WarehouseDBSettings
        {
            ConnectionString = _runner.ConnectionString,
            DatabaseName = "TestDatabase",
            OrdersCollectionName = "orders",
            CategoriesCollectionName = "categories",
            CounterCollectionName = "counters",
            ProductsCollectionName = "products"
        });

        WarehouseDbContext = new WarehouseDbContext(Settings);

        MongoMapping.RegisterMappings();
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}
