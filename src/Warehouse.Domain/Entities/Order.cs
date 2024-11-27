using Warehouse.Domain.Enums;

namespace Warehouse.Domain.Entities;

public class Order
{
    public string Id { get; set; }

    public required int ProductId { get; set; }

    public required int Quantity { get; set; }

    public required OrderStatus Status { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ApprovalDate { get; set; }
}
