using MongoDB.Bson.Serialization;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Configurations;

internal static class CounterDbMapping
{
    internal static void Register()
    {
        BsonClassMap.RegisterClassMap<CounterDb>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id);
            cm.GetMemberMap(c => c.SequenceValue)
                        .SetElementName("seq_value")
                        .SetOrder(1)
                        .SetIsRequired(true);
            cm.SetIgnoreExtraElements(true);
        });
    }
}
