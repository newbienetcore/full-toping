namespace Shared.MessageBroker.RabbitMQ.Extensions;

public static class MessageQueueSettingExtensions
{
    public static string GetPublishEndpoint(this MessageQueueSetting setting, string endpoint)
    {
        var virHost = setting.VirtualHost.Trim().TrimStart('/').TrimEnd('/');
        virHost = string.IsNullOrWhiteSpace(virHost) ? virHost : $"/{virHost}";
        return $"rabbitmq://{setting.Host}:{setting.Port}{virHost}/{endpoint}";
    }

    public static string GetPublishEndpointError(this MessageQueueSetting setting, string endpoint)
    {
        var virHost = setting.VirtualHost.Trim().TrimStart('/').TrimEnd('/');
        virHost = string.IsNullOrWhiteSpace(virHost) ? virHost : $"/{virHost}";
        return $"rabbitmq://{setting.Host}:{setting.Port}{virHost}/{endpoint}_error";
    }

    public static string GetReceiveEndpoint(this MessageQueueSetting setting, string endpoint)
    {
        return $"{setting.QueuePrefix}{endpoint}";
    }

    public static string GetReceiveEndpointError(this MessageQueueSetting setting, string endpoint)
    {
        return $"{setting.QueuePrefix}{endpoint}_error";
    }

    public static Uri GetEndpointAddress(this MessageQueueSetting setting, string endpoint)
    {
        return new Uri(setting.GetPublishEndpoint(endpoint));
    }
}