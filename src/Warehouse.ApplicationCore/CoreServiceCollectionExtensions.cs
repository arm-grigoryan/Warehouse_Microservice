using Microsoft.Extensions.DependencyInjection;
using Warehouse.ApplicationCore.Orders.DTOs;
using Warehouse.ApplicationCore.Orders.Mapping;
using Warehouse.ApplicationCore.Services.AvailableStockOrders;
using Warehouse.ApplicationCore.Services.LowStockOrders;
using Warehouse.ApplicationCore.Services.PendingOrders;

namespace Warehouse.ApplicationCore;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(OrdersMappingProfile));

        services.AddMediatR(cf => cf.RegisterServicesFromAssemblyContaining<OrderDto>());

        services.AddScoped<IAvailableStockOrdersService, AvailableStockOrdersService>();
        services.AddScoped<ILowStockOrdersService, LowStockOrdersService>();
        services.AddScoped<IPendingOrdersService, PendingOrdersService>();

        return services;
    }
}
