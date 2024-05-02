using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using SharedKernel.ApiGateway;


namespace SharedKernel.Configure
{
    public static partial class ConfigureExtension
    {
        #region ApiGateway

        [Obsolete]
        public static IWebHostBuilder ConfigCoreApiGateway(this IWebHostBuilder builder) =>
            builder.ConfigureKestrel(serverOptions => { serverOptions.Limits.MaxRequestBodySize = int.MaxValue; })
                .ConfigureAppConfiguration(config =>
                    config.AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("ocelot.json"))
                .ConfigureServices(services =>
                {
                    services.AddSingleton(builder);
                    services.AddOcelot().AddPolly();
                    services.AddCors();
                })
                .UseCoreSerilog()
                .Configure(app =>
                {
                    app.UseCoreExceptionHandler();
                    app.UseCoreCors(app.ApplicationServices.GetRequiredService<IConfiguration>());
                    app.UseMiddleware<RequestResponseLoggingMiddleware>();
                    app.UseOcelot().Wait();
                });

        #endregion
    }
}