using System.Globalization;
using System.Text;
using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using RabbitMQ.Client;
using Serilog;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Core;
using SharedKernel.Runtime;

namespace SharedKernel.Configures;

public static partial class ConfigureExtension
{
    #region Dependency Injection

    public static IServiceCollection AddCoreLocalization(this IServiceCollection services)
    {
        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(culture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
        });

        return services;
    }


    public static IServiceCollection AddCoreAuthentication(this IServiceCollection services,
        IConfiguration Configuration)
    {
        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = Configuration["Auth:JwtSettings:Issuer"],
                ValidateIssuer = true,
                ValidIssuer = Configuration["Auth:JwtSettings:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:JwtSettings:Key"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            jwtOptions.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/socket-message"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }


    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddCoreRateLimit(this IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.RealIpHeader = HeaderNamesExtension.RealIpHeader;
            options.ClientIdHeader = HeaderNamesExtension.ClientIdHeader;
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1s",
                    Limit = 8,
                }
            };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }

    #endregion

    #region Middlewares

    public static void UseCoreCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("Allowedhosts").Value;
        if (origins.Equals("*"))
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseCors(x => x.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
        }
    }

    public static void UseCoreLocalization(this IApplicationBuilder app)
    {
        //app.Use(async (context, next) =>
        //{
        //    var culture = context.Request.Headers[HeaderNames.AcceptLanguage].ToString();
        //    switch (culture)
        //    {
        //        case "vi":
        //            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
        //            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");
        //            break;
        //        default:
        //            break;
        //    }

        //    await next();
        //});

        var supportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("vi-VN") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }

    public static void UseCoreHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = (HealthCheckRegistration _) => true,
            ResponseWriter = new Func<HttpContext, HealthReport, Task>(UIResponseWriter.WriteHealthCheckUIResponse)
        });
    }

    #endregion

    #region Logging

    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

            configuration
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        };
    

    #endregion
    
}