using System.Reflection;
using Caching;
using Catalog.Api.Persistence;
using Catalog.Api.Repositories;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SharedKernel.Auth;
using SharedKernel.Configures;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Infrastructures;

namespace Catalog.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => configuration);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCoreLocalization();
        
        services.AddControllers();

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureDbContext(configuration);

        services.AddCoreCaching(configuration);
        
        services.AddInfrastructureServices();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MasterDb");
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("Connection string is not configured.");
        }
        
        var builder = new MySqlConnectionStringBuilder(connectionString);
        services.AddDbContext<CatalogDbContext>(m => m.UseMySql(builder.ConnectionString, 
            ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Catalog.Api");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }
    
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(typeof(IEFCoreReadOnlyRepository<,,>), typeof(EFCoreReadOnlyRepository<,,>))
            .AddScoped(typeof(IEFCoreWriteOnlyRepository<,,>), typeof(EFCoreWriteOnlyRepository<,,>))
            .AddScoped<IProductRepository, ProductRepository>();
    }
}