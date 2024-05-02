using MassTransit;
using Shared.MessageBroker.Abstractions;

namespace Shared.MessageBroker.RabbitMQ;

public class MasstransitMessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MasstransitMessagePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Publish<T>(T message, CancellationToken cancellationToken = default, Dictionary<string, string>? metaData = null) where T : class
    {
        await _publishEndpoint.Publish(message, ctx =>
        {
            if (metaData?.Any() ?? false)
            {
                foreach (var hKey in metaData.Keys)
                {
                    ctx.Headers.Set(hKey, metaData[hKey], false);
                }
            }
        }, cancellationToken);
    }
}