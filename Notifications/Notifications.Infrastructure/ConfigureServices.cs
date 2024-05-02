using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Configurations;
using Notifications.Domain.Entities;
using Notifications.Infrastructure.Identity;
using Notifications.Infrastructure.Persistence;
using Notifications.Infrastructure.Repositories.NotificationGroups;
using Notifications.Infrastructure.Services;
using Shared.Caching.Redis.Extensions;
using Shared.Utilities;

namespace Notifications.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseSetting = configuration.GetOptions<DatabaseSetting>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(databaseSetting.Default));

        services.AddScoped<ApplicationDbContextSeed>();
        
        
        services
            .AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddRedisCaching(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCaching(configuration);
        return services;
    }
}