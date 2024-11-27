using MassTransit;

namespace Warehouse.Infrastructure.Sagas;

public class OrderReviewingSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public string OrderId { get; set; }
    public string StockReservationId { get; set; }
}
