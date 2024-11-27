namespace Warehouse.API.Contracts.Products;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string CategoryId { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public int OutOfStockThreshold { get; set; }
}
