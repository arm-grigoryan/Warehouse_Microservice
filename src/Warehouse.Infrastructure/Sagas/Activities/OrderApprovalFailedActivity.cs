using MassTransit;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.Infrastructure.Sagas.Activities;

public class OrderApprovalFailedActivity(
    IProductsRepository productsRepository,
    IOrdersRepository ordersRepository) : IStateMachineActivity<OrderReviewingSagaData, Fault<OrderApprovalRequested>>
{
    public async Task Execute(BehaviorContext<OrderReviewingSagaData, Fault<OrderApprovalRequested>> context, IBehavior<OrderReviewingSagaData, Fault<OrderApprovalRequested>> next)
    {
        // Roll back product stock
        await productsRepository.UpdateStockAsync(context.Message.Message.ProductId, context.Message.Message.CurrentProductStock, context.CancellationToken);

        // update order status
        await ordersRepository.UpdateStatusAsync(context.Message.Message.OrderId, OrderStatus.Declined, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderReviewingSagaData, Fault<OrderApprovalRequested>, TException> context, IBehavior<OrderReviewingSagaData, Fault<OrderApprovalRequested>> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-approved-failed");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }
}
