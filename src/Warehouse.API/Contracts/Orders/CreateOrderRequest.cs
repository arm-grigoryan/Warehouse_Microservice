﻿namespace Warehouse.API.Contracts.Orders;

public class CreateOrderRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
