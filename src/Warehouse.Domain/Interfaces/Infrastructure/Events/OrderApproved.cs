namespace Warehouse.Domain.Interfaces.Infrastructure.Events;

public class OrderApproved
{
    public Guid CorrelationId { get; set; }
    public string OrderId { get; set; }
    public int Quantity { get; set; }
}
