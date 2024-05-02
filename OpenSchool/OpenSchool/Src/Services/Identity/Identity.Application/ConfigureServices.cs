using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Add  FluentValidator
        services.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;

            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
                
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        });
        
        // Add UseCase
        
        
        return services;
    }
    
}