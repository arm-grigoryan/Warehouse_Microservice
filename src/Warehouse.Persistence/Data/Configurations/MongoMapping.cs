namespace Warehouse.Persistence.Data.Configurations;

internal static class MongoMapping
{
    private static readonly object LockObject = new();
    private static bool _isRegistered = false;

    internal static void RegisterMappings()
    {
        if (_isRegistered)
            return;

        lock (LockObject)
        {
            if (_isRegistered)
                return;

            ProductDbMapping.Register();
            OrderDbMapping.Register();
            PendingOrderDbMapping.Register();
            CategoryDbMapping.Register();
            CounterDbMapping.Register();

            _isRegistered = true;
        }
    }
}
