namespace Notifications.WebApi;

public static class ConfigureApplication
{
    public static IApplicationBuilder UseMonitoring(this WebApplication app)
    {


        return app;
    }
}