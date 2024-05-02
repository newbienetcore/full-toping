using Contracts.Common.Interfaces;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"), builder =>
            {
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });
        
        services.AddScoped<ApplicationDbContextSeed>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));
        
        return services;
    }
}