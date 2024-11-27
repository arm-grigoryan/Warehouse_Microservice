﻿using Warehouse.Domain.Enums;

namespace Warehouse.Persistence.Data.Entities;

internal class OrderDb
{
    public string Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ApprovalDate { get; set; }
}
