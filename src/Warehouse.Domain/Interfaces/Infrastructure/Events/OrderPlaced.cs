namespace Warehouse.Domain.Interfaces.Infrastructure.Events;

public class OrderPlaced
{
    public Guid CorrelationId { get; set; }
    public string OrderId { get; set; }
    public int ProductId { get; set; }
    public int CurrentProductStock { get; set; }
    public int Quantity { get; set; }
}
