namespace Shared.MessageBroker.Abstractions;

public interface INotificationGroupPublisher
{
    Task Publish(
        string code,
        string name,
        Guid? parentId,
        string[]? variables,
        string[]? supportedUserLevel,
        Guid appId,
        CancellationToken cancellationToken = default);
}