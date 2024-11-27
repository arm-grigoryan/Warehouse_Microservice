using MassTransit;
using Warehouse.Domain.Interfaces.Infrastructure;

namespace Warehouse.Infrastructure.MassTransit;

internal class MassTransitMessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return publishEndpoint.Publish(message, cancellationToken);
    }
}
