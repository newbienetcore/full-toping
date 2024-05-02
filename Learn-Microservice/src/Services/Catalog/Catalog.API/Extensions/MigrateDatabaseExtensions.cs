using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Extensions;

public static class MigrateDatabaseExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Migrating mysql database.");
                ExecuteMigrations<TContext>(context);
                logger.LogInformation("Migrated mysql database.");
                InvokeSeeder(seeder, context, services);
            }
            catch(Exception exception)
            {
                logger.LogError(exception, "An error occurred while migrating the mysql database.");
            }
        }

        return host;
    }

    private static void ExecuteMigrations<TContext>(TContext context)
        where TContext : DbContext
    {
        context.Database.Migrate();
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services)
    {
        seeder(context, services);
    }
}