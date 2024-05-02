using System.Reflection;
using Catalog.API.Persistence;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Shared;
using Shared.Middlewares;

namespace Catalog.API;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen();
        
        services.ConfigureApplicationDbContext(configuration);

        services.AddInfrastructureService(configuration);
        
        services.AddFluentValidatorService();
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<HandleExceptionMiddleware>();
        
        
        return services;
    }

    #region [PRIVATE METHODS]

    private static IServiceCollection ConfigureApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        // get connectionString trong file appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // build ra một connection string chuẩn cho mysql
        var builder = new MySqlConnectionStringBuilder(connectionString);

        // ServerVersion.AutoDetect(builder.ConnectionString): là một phương thức được sử dụng để tự động xác định phiên bản của cơ sở dữ liệu SQL Server dựa trên chuỗi kết nối (connection string) được cung cấp.
        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseMySql(builder.ConnectionString, ServerVersion.AutoDetect(builder.ConnectionString),
                contextOptionsBuilder =>
                {
                    contextOptionsBuilder.MigrationsAssembly("Catalog.API");
                    contextOptionsBuilder.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                });
        });
        

        return services;
    }
    
    private static IServiceCollection AddInfrastructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
        services.AddScoped(typeof(IBrandRepository), typeof(BrandRepository));
        services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));


        return services;
    }
    
    #endregion
}