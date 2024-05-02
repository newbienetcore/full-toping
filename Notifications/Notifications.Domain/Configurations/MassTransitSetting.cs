namespace Notifications.Domain.Configurations;

public class MassTransitSetting
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ClientProvidedName { get; set; }
}