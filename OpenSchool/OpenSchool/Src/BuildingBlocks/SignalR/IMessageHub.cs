namespace SignalR;

public interface IMessageHub
{
    Task ReceiveMessageAsync(string message, CancellationToken cancellationToken = default);

    Task GetMessageAsync(CancellationToken cancellationToken = default);
}