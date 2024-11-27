namespace Warehouse.Persistence.Settings;

public class WarehouseDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ProductsCollectionName { get; set; }
    public string OrdersCollectionName { get; set; }
    public string PendingOrdersCollectionName { get; set; }
    public string CategoriesCollectionName { get; set; }
    public string CounterCollectionName { get; set; }
}
