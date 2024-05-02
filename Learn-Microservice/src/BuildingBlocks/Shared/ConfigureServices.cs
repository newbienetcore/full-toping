using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared;

public static class ConfigureServices
{
    public static IServiceCollection AddFluentValidatorService(this IServiceCollection services)
    {
        services.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;

            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
                
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        });

        return services;
    }
}