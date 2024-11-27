using MassTransit;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Infrastructure.Sagas.Activities;
namespace Warehouse.Infrastructure.Sagas;

internal class OrderReviewingSagaStateMachine : MassTransitStateMachine<OrderReviewingSagaData>
{
    public State WaitingForApproval { get; private set; }
    public State Approved { get; private set; }
    public State Declined { get; private set; }

    public Event<OrderPlaced> OrderPlaced { get; private set; }
    public Event<OrderApproved> OrderApproved { get; private set; }
    public Event<OrderDeclined> OrderDeclined { get; private set; }
    public Event<Fault<OrderApprovalRequested>> OrderApprovalFailed { get; private set; }

    public OrderReviewingSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderPlaced, e => e.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderApproved, e => e.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderDeclined, e => e.CorrelateById(x => x.Message.CorrelationId));
        Event(() => OrderApprovalFailed, e => e.CorrelateById(x => x.Message.Message.CorrelationId));

        Initially(
            When(OrderPlaced)
                .Activity(x => x.OfType<ReserveStockActivity>())
                .TransitionTo(WaitingForApproval)
                .Publish(context => new OrderApprovalRequested
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderId = context.Message.OrderId,
                    ProductId = context.Message.ProductId,
                    CurrentProductStock = context.Message.CurrentProductStock,
                    Quantity = context.Message.Quantity
                })
        );

        During(WaitingForApproval,
            When(OrderApproved)
                .Activity(x => x.OfType<OrderApprovedActivity>())
                .TransitionTo(Approved)
                .Finalize(),
            When(OrderDeclined)
                .Activity(x => x.OfType<OrderDeclinedActivity>())
                .TransitionTo(Declined)
                .Finalize(),
            When(OrderApprovalFailed)
                .Activity(x => x.OfType<OrderApprovalFailedActivity>())
                .TransitionTo(Declined)
                .Finalize()
        );
    }
}
