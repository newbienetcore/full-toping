using Microsoft.Extensions.Configuration;
using Serilog;

namespace Common.Log;

public class DefaultLoggingConfig
{
    public static string ApplicationName { get; private set; }
    
    public static ILogger Logger { get; private set; }
    
    public static void SetConfig(IConfiguration configuration, ILogger logger)
    {
        var applicationName = configuration.GetValue<string>("ApplicationName");
        if (!string.IsNullOrEmpty(applicationName))
        {
            ApplicationName = applicationName;
        }
        Logger = logger;
    }
}