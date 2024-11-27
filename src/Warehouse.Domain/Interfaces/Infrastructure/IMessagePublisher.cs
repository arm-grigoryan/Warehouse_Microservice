namespace Warehouse.Domain.Interfaces.Infrastructure;

public interface IMessagePublisher
{
    Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class;
}
