using Warehouse.Domain.Enums;
using Warehouse.Domain.Exceptions;

namespace Warehouse.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string CategoryId { get; set; }

    private int _stock;
    public required int Stock
    {
        get => _stock;
        set
        {
            if (value < 0)
            {
                throw new WarehouseException("Stock cannot be negative.");
            }
            _stock = value;
        }
    }
    public required int LowStockThreshold { get; set; }
    public required int OutOfStockThreshold { get; set; }
    public StockStatus Status => GetStockStatus();
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    private StockStatus GetStockStatus()
    {
        if (Stock < OutOfStockThreshold) return StockStatus.OutOfStock;
        if (Stock < LowStockThreshold) return StockStatus.LowStock;
        return StockStatus.Available;
    }
}