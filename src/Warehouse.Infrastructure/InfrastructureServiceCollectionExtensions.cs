using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Domain.Interfaces.Infrastructure;
using Warehouse.Infrastructure.MassTransit;
using Warehouse.Infrastructure.MassTransit.Settings;
using Warehouse.Infrastructure.Sagas;
using Warehouse.Infrastructure.Sagas.Activities;

namespace Warehouse.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();
        services.AddRabbitMQ(configuration);

        return services;
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<OrderReviewingSagaStateMachine, OrderReviewingSagaData>()
                .InMemoryRepository();

            // Add Activities
            services.AddScoped<ReserveStockActivity>();
            services.AddScoped<OrderApprovedActivity>();
            services.AddScoped<OrderDeclinedActivity>();
            services.AddScoped<OrderApprovalFailedActivity>();

            // Add consumers
            x.AddConsumer<OrderApprovalRequestConsumer>();

            // RabbitMQ Transport Configuration
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitSettings.Host), h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });

                cfg.UseMessageRetry(r =>
                {
                    r.Interval(rabbitSettings.RetryCount, TimeSpan.FromSeconds(rabbitSettings.RetryIntervalSeconds));
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}