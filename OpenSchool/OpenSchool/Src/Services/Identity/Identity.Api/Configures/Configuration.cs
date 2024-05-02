using Logging;

namespace Identity.Api.Configures;

public static class Configuration
{
    public static WebApplicationBuilder AddCoreWebApplication(this WebApplicationBuilder builder)
    {
        // add app settings
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName }.json", optional: true, reloadOnChange: true);
        
        // add environment variables
        builder.Configuration.AddEnvironmentVariables();

        builder.Host.UseCoreSerilog();
        
        return builder;
    }
}