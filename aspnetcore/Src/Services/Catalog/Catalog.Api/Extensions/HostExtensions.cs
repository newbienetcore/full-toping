using Microsoft.EntityFrameworkCore;
using SharedKernel.EFCore;

namespace Catalog.Api.Extensions;

public static class HostExtensions
{
    public static void AddAppConfiguration(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment.EnvironmentName;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }

    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : CoreDbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            
            try
            {
                logger.LogInformation("Migrating mysql database.");
                ExecuteMigrations(context);
                logger.LogInformation("Migrated mysql database.");
                InvokeSeeder(seeder, context, services);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the mysql database");
            }
        }

        return host;
    }
    
    private static void ExecuteMigrations<TContext>(TContext context)
        where TContext : CoreDbContext
    {
        context.Database.Migrate();
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : CoreDbContext
    {
        seeder(context, services);
    }
}