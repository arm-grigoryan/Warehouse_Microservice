using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Configurations;

internal static class OrderDbMapping
{
    internal static void Register()
    {
        BsonClassMap.RegisterClassMap<OrderDb>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
            cm.GetMemberMap(c => c.ProductId)
                        .SetElementName("product_id")
                        .SetOrder(1)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.Quantity)
                        .SetElementName("quantity")
                        .SetOrder(2)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.Status)
                        .SetElementName("status")
                        .SetOrder(3)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.OrderDate)
                        .SetElementName("order_date")
                        .SetOrder(4)
                        .SetIsRequired(true)
                        .SetDefaultValue(DateTime.UtcNow);
            cm.GetMemberMap(c => c.ApprovalDate)
                        .SetElementName("approval_date")
                        .SetOrder(5);
            cm.SetIgnoreExtraElements(true);
        });
    }
}
