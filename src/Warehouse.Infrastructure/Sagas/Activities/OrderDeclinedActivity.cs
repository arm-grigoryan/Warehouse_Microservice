using MassTransit;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.Infrastructure.Sagas.Activities;

public class OrderDeclinedActivity(
    IProductsRepository productsRepository,
    IOrdersRepository ordersRepository) : IStateMachineActivity<OrderReviewingSagaData, OrderDeclined>
{
    public async Task Execute(BehaviorContext<OrderReviewingSagaData, OrderDeclined> context, IBehavior<OrderReviewingSagaData, OrderDeclined> next)
    {
        // Roll back product stock
        await productsRepository.UpdateStockAsync(context.Message.ProductId, context.Message.InitialProductStock, context.CancellationToken);

        // update order status
        await ordersRepository.UpdateStatusAsync(context.Message.OrderId, OrderStatus.Declined, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderReviewingSagaData, OrderDeclined, TException> context, IBehavior<OrderReviewingSagaData, OrderDeclined> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-declined");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }
}
