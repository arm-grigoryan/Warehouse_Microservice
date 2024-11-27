namespace Warehouse.API.Contracts.Products;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CategoryId { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public int OutOfStockThreshold { get; set; }
    public DateTime CreatedDate { get; set; }
}
