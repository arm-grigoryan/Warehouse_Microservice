namespace Warehouse.Infrastructure.MassTransit.Settings;

internal class RabbitMQSettings
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int RetryCount { get; set; }
    public int RetryIntervalSeconds { get; set; }
}
