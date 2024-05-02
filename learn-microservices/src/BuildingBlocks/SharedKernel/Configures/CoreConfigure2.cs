using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedKernel.Core;

namespace SharedKernel.Configure
{
    public static partial class ConfigureExtension
    {
        #region ELK

        public static IHostBuilder UseCoreSerilog(this IHostBuilder builder) => builder.UseSerilog(
                (context, loggerConfiguration) =>
                {
                    // CoreSettings.SetElasticSearchConfig(context.Configuration);
                    loggerConfiguration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Application", DefaultElasticSearchConfig.ApplicationName)
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
                        .WriteTo.Elasticsearch(
                            new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                                new Uri(DefaultElasticSearchConfig.Uri))
                            {
                                IndexFormat = $"{DefaultElasticSearchConfig.ApplicationName}-{DateTime.UtcNow:yyyy-MM}",
                                AutoRegisterTemplate = true,
                                NumberOfReplicas = 1,
                                NumberOfShards = 2,
                                ModifyConnectionSettings = x =>
                                    x.BasicAuthentication(DefaultElasticSearchConfig.Username,
                                        DefaultElasticSearchConfig.Password)
                            })
                        .ReadFrom.Configuration(context.Configuration);
                })
            .ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                var logger = sp.GetRequiredService<ILogger>();
                var configuration = sp.GetRequiredService<IConfiguration>();

                CoreSettings.SetLoggingConfig(configuration, logger);
            });

        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
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


        [Obsolete]
        public static IWebHostBuilder UseCoreSerilog(this IWebHostBuilder builder) =>
            builder.UseSerilog((context, loggerConfiguration) =>
                {
                    CoreSettings.SetElasticSearchConfig(context.Configuration);
                    loggerConfiguration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Application", DefaultElasticSearchConfig.ApplicationName)
                        .WriteTo.Console(
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
                        .WriteTo.Elasticsearch(
                            new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                                new Uri(DefaultElasticSearchConfig.Uri))
                            {
                                IndexFormat = $"{DefaultElasticSearchConfig.ApplicationName}-{DateTime.UtcNow:yyyy-MM}",
                                AutoRegisterTemplate = true,
                                NumberOfReplicas = 1,
                                NumberOfShards = 2,
                                ModifyConnectionSettings = x =>
                                    x.BasicAuthentication(DefaultElasticSearchConfig.Username,
                                        DefaultElasticSearchConfig.Password)
                            })
                        .ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();
                    var logger = sp.GetRequiredService<ILogger>();
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    CoreSettings.SetLoggingConfig(configuration, logger);
                });

        #endregion
    }
}