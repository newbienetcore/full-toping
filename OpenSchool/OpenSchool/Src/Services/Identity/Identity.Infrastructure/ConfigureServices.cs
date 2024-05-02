using Identity.Application.Persistence;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Core;

namespace Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext, DbContextSeed
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(CoreSettings.ConnectionStrings["IdentityDb"])
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true)
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
        );
        
        services.AddScoped<ApplicationDbContextSeed>();
        
        // Add DI Repositories
        

        return services;
    }
}