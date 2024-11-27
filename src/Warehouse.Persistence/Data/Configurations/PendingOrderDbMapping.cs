using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Warehouse.Persistence.Data.Entities;

namespace Warehouse.Persistence.Data.Configurations;

internal class PendingOrderDbMapping
{
    internal static void Register()
    {
        BsonClassMap.RegisterClassMap<PendingOrderDb>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });
    }
}
