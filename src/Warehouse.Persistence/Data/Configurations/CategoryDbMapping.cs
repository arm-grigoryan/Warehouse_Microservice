using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Configurations;

internal static class CategoryDbMapping
{
    internal static void Register()
    {
        BsonClassMap.RegisterClassMap<CategoryDb>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
            cm.GetMemberMap(c => c.Name)
                        .SetElementName("name")
                        .SetOrder(1)
                        .SetIsRequired(true);
            cm.GetMemberMap(c => c.Description)
                        .SetElementName("description")
                        .SetOrder(2)
                        .SetIsRequired(true);
            cm.SetIgnoreExtraElements(true);
        });
    }
}
