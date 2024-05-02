using Catalog.Application.Persistence;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using Catalog.Infrastructure.Configs;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        #region Core settings projects
        
        FirebaseConfig.SetConfig(configuration);
        
        #endregion
        
        services.AddDbContextPool<ApplicationDbContext>((provider, options) =>
        {
            options.UseMySql(
                    connectionString: CoreSettings.ConnectionStrings["MasterDb"],
                    serverVersion: ServerVersion.AutoDetect(CoreSettings.ConnectionStrings["MasterDb"]))
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true);
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<ApplicationDbContextSeed>();

        #region Services

        services.AddScoped<ICachingService, CachingService>();
        
        services.AddSingleton<IFileService, FileService>();
        
        services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

        #endregion
        
        // Base
        services.AddScoped(typeof(IEfCoreReadOnlyRepository<,>), typeof(EfCoreReadOnlyRepository<,>));
        services.AddScoped(typeof(IEfCoreWriteOnlyRepository<,>), typeof(EfCoreWriteOnlyRepository<,>));
        
        // Supplier
        services.AddScoped<ISupplierWriteOnlyRepository, SupplierWriteOnlyRepository>();
        services.AddScoped<ISupplierReadOnlyRepository, SupplierReadOnlyRepository>();
        
        // Category
        services.AddScoped<ICategoryWriteOnlyRepository, CategoryWriteOnlyRepository>();
        services.AddScoped<ICategoryReadOnlyRepository, CategoryReadOnlyRepository>();
        
        // Location
        services.AddScoped<ILocationReadOnlyRepository, LocationReadOnlyRepository>();
        
        // Asset
        services.AddScoped<IAssetReadOnlyRepository, AssetReadOnlyRepository>();
        services.AddScoped<IAssetWriteOnlyRepository, AssetWriteOnlyRepository>();
        
        // Attribute
        services.AddScoped<IAttributeReadOnlyRepository, AttributeReadOnlyRepository>();
        services.AddScoped<IAttributeWriteOnlyRepository, AttributeWriteOnlyRepository>();

        return services;
    }
}