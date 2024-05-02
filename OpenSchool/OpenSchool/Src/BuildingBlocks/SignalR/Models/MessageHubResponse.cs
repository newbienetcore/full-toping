using System.ComponentModel.DataAnnotations;

namespace SignalR;

public class MessageHubResponse
{
    public MessageHubType Type { get; set; } = MessageHubType.Message;
    
    [Required]
    public object Message { get; set; }
}