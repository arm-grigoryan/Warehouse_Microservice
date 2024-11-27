using MassTransit;
using Warehouse.Domain.Interfaces.Infrastructure;
using Warehouse.Domain.Interfaces.Infrastructure.Events;

namespace Warehouse.Infrastructure.MassTransit;

internal class OrderApprovalRequestConsumer(IMessagePublisher messagePublisher) : IConsumer<OrderApprovalRequested>
{
    public async Task Consume(ConsumeContext<OrderApprovalRequested> context)
    {
        var message = context.Message;

        var isApproved = new Random().Next(0, 2) == 0;

        if (isApproved)
        {
            await messagePublisher.Publish(new OrderApproved
            {
                CorrelationId = message.CorrelationId,
                OrderId = message.OrderId,
                Quantity = message.Quantity
            });
        }
        else
        {
            await messagePublisher.Publish(new OrderDeclined
            {
                CorrelationId = message.CorrelationId,
                OrderId = message.OrderId,
                ProductId = message.ProductId,
                InitialProductStock = message.CurrentProductStock,
                Quantity = message.Quantity
            });
        }
    }
}
