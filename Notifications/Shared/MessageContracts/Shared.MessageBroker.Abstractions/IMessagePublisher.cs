namespace Shared.MessageBroker.Abstractions;

public interface IMessagePublisher
{
    Task Publish<T>(
        T message,
        CancellationToken cancellationToken = default,
        Dictionary<string, string>? metaData = null) where T : class;
}