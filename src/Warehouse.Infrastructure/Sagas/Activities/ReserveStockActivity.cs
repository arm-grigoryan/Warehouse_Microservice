using MassTransit;
using Warehouse.Domain.Interfaces.Infrastructure.Events;
using Warehouse.Domain.Interfaces.Persistence;

namespace Warehouse.Infrastructure.Sagas.Activities;

public class ReserveStockActivity(
    IProductsRepository productsRepository) : IStateMachineActivity<OrderReviewingSagaData, OrderPlaced>
{
    public async Task Execute(BehaviorContext<OrderReviewingSagaData, OrderPlaced> context, IBehavior<OrderReviewingSagaData, OrderPlaced> next)
    {
        // Update product stock
        var newStock = context.Message.CurrentProductStock - context.Message.Quantity;
        await productsRepository.UpdateStockAsync(context.Message.ProductId, newStock, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderReviewingSagaData, OrderPlaced, TException> context, IBehavior<OrderReviewingSagaData, OrderPlaced> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-placed");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }
}
