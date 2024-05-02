using System.Text.Json.Serialization;

namespace MessageBroker.Abstractions.Events;

public record IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; set; }

    [JsonInclude]
    public DateTime Timestamp { get; protected set; }
    
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.Now;
    }
}

