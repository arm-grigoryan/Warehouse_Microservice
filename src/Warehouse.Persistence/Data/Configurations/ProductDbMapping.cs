using MongoDB.Bson.Serialization;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Configurations;

internal static class ProductDbMapping
{
    internal static void Register()
    {
        BsonClassMap.RegisterClassMap<ProductDb>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.Id)
                        .SetElementName("_id")
                        .SetOrder(0)
                        .SetIdGenerator(null);
            cm.GetMemberMap(c => c.Name)
                        .SetElementName("name")
                        .SetOrder(1)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.CategoryId)
                        .SetElementName("category_id")
                        .SetOrder(2)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.Stock)
                        .SetElementName("stock")
                        .SetOrder(3)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.LowStockThreshold)
                        .SetElementName("low_stock_threshold")
                        .SetOrder(4)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.OutOfStockThreshold)
                        .SetElementName("out_of_stock_threshold")
                        .SetOrder(5)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.CreatedDate)
                        .SetElementName("created_date")
                        .SetOrder(6)
                        .SetIsRequired(true)
                        .SetDefaultValue(DateTime.UtcNow);
            cm.GetMemberMap(c => c.UpdatedDate)
                        .SetElementName("updated_date")
                        .SetOrder(7);
            cm.SetIgnoreExtraElements(true);
        });
    }
}
