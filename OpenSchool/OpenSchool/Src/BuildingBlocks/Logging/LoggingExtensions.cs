using Microsoft.Extensions.Hosting;
using Serilog;

namespace Logging;

public static class LoggingExtensions
{
    public static IHostBuilder UseCoreSerilog(this IHostBuilder builder)
        => builder.UseSerilog(Logging.Configure);
}