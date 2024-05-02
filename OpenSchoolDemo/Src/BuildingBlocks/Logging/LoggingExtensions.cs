using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Log;

public static class LoggingExtensions
{
    public static IHostBuilder UseCoreSerilog(this IHostBuilder builder) 
        => builder.UseSerilog(Logging.Configure)
            .ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                var logger = sp.GetRequiredService<ILogger>();
                var configuration = sp.GetRequiredService<IConfiguration>();

                DefaultLoggingConfig.SetConfig(configuration, logger);
            });
}