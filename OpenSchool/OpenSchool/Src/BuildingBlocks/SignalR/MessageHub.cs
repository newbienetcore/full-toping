using Microsoft.AspNetCore.SignalR;
using Serilog;
using SharedKernel.Auth;
using SharedKernel.Contracts;

namespace SignalR;

public class MessageHub : Hub
{ 
    public static Dictionary<string, List<string>> Connections = new Dictionary<string, List<string>>();
    public static Dictionary<string, string> KeyValueConnections = new Dictionary<string, string>();

    private readonly IHubContext<MessageHub> _hubContext;
    private readonly ICurrentUser _currentUser;

    public MessageHub(IHubContext<MessageHub> hubContext, ICurrentUser currentUser)
    {
      _hubContext = hubContext;
      _currentUser = currentUser;
    }

    [HubMethodName("SendMessageToCaller")]
    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
      await Clients.Caller
          .SendAsync(
              method: "ReceiveMessage",
              arg1: message, 
              cancellationToken: cancellationToken);
    }

    [HubMethodName("SendMessageToClients")]
    public async Task SendMessagesAsync(NotificationMessage notification, CancellationToken cancellationToken = default)
    {
        try
        {
            if (notification == null)
            {
                return;
            }

            if (notification.IsAllClients)
            {
                await _hubContext.Clients.All
                    .SendAsync(
                        method: "ReceiveMessage", 
                        arg1: new MessageHubResponse { Type = notification.Type, Message = notification.Description }, 
                        cancellationToken: cancellationToken);
                return;
            }
            foreach (var key in notification.Keys)
            {
                if (MessageHub.Connections.TryGetValue(key, out var connectionIds))
                {
                    await _hubContext.Clients.Clients(connectionIds)
                        .SendAsync(
                            method: "ReceiveMessage", 
                            arg1: new MessageHubResponse { Type = notification.Type, Message = notification.Description }, 
                            cancellationToken: cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }
    }

    public async Task SendNumberOfOnlineUsersAsync(CancellationToken cancellationToken = default)
    {
      await Clients.All.SendAsync(
          method: "ReceiveMessage", 
          arg1: new MessageHubResponse { Type = MessageHubType.OnlineUser, Message = KeyValueConnections.Count },
          cancellationToken: cancellationToken);
    }
   
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var claims = (Context.User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims;
        if (!claims.Any())
        {
            KeyValueConnections.TryAdd(Context.ConnectionId, Context.ConnectionId);
        }
        else
        {
            var owner = claims.First(x => x.Type == ClaimConstant.USER_ID);
            var key = $"owner_{owner.Value}";

            if (Connections.TryGetValue(key, out var connectionIds))
            {
                connectionIds.Add(Context.ConnectionId);
                Connections[key] = connectionIds;
            }
            else
            {
                Connections[key] = new List<string>
                {
                    Context.ConnectionId
                };
            }
            KeyValueConnections.TryAdd(Context.ConnectionId, key);
        }
        await SendNumberOfOnlineUsersAsync(default);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        if (KeyValueConnections.TryGetValue(Context.ConnectionId, out var value))
        {
            if (Connections.ContainsKey(value))
            {
                Connections[value].Remove(Context.ConnectionId);
            }
            KeyValueConnections.Remove(Context.ConnectionId);
            await SendNumberOfOnlineUsersAsync(default);
        }
    }
}