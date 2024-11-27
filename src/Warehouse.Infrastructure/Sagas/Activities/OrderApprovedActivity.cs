using MassTransit;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.Infrastructure.Sagas.Activities;

public class OrderApprovedActivity(IOrdersRepository ordersRepository) : IStateMachineActivity<OrderReviewingSagaData, OrderApproved>
{
    public async Task Execute(BehaviorContext<OrderReviewingSagaData, OrderApproved> context, IBehavior<OrderReviewingSagaData, OrderApproved> next)
    {
        // update order status
        await ordersRepository.UpdateStatusAsync(context.Message.OrderId, OrderStatus.Approved, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderReviewingSagaData, OrderApproved, TException> context, IBehavior<OrderReviewingSagaData, OrderApproved> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-approved");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }
}
