namespace Warehouse.Domain.Entities;

public class Category
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}