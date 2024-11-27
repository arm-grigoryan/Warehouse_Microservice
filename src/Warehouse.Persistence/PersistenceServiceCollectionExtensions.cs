using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Domain.Interfaces.Persistence;
using Warehouse.Persistence.Data;
using Warehouse.Persistence.Data.Configurations;
using Warehouse.Persistence.Data.Mappings;
using Warehouse.Persistence.Data.Repositories;
using Warehouse.Persistence.Settings;

namespace Warehouse.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(DbEntitiesMappings));

        services.Configure<WarehouseDBSettings>(configuration.GetSection(nameof(WarehouseDBSettings)));

        services.AddSingleton<WarehouseDbContext>();
        services.AddScoped<CounterService>();
        MongoMapping.RegisterMappings();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IPendingOrdersRepository, PendingOrdersRepository>();

        return services;
    }
}