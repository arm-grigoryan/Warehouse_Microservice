namespace Warehouse.Domain.Interfaces.Infrastructure.Events;

public class OrderDeclined
{
    public Guid CorrelationId { get; set; }
    public string OrderId { get; set; }
    public int ProductId { get; set; }
    public int InitialProductStock { get; set; }
    public int Quantity { get; set; }
}
