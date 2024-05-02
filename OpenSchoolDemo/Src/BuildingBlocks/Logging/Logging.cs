using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Log;

public class Logging
{
    public static void Information(string information)
    {
        DefaultLoggingConfig.Logger.Information(information);
    }

    public static void Warning(string information)
    {
        DefaultLoggingConfig.Logger.Warning(information);
    }

    public static void Error(Exception exception, string messageTemplate = "")
    {
        DefaultLoggingConfig.Logger.Error(exception, messageTemplate);
    }

    public static void Error(string information)
    {
        DefaultLoggingConfig.Logger.Error(information);
    }
    
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var applicationName = !string.IsNullOrEmpty(DefaultLoggingConfig.ApplicationName) ? DefaultLoggingConfig.ApplicationName : context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

            configuration
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        };
}