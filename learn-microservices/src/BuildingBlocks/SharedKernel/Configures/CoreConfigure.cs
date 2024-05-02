using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using KSharedKernel.RabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using RabbitMQ.Client;
using Serilog;
using SharedKernel.ApiGateway;
using SharedKernel.Application;
using SharedKernel.Auth;
using SharedKernel.Core;
using SharedKernel.Infrastructures;
using SharedKernel.Libraries;
using SharedKernel.Log;
using SharedKernel.Middlewares;
using SharedKernel.MongoDB;
using SharedKernel.MySQL;
using SharedKernel.Properties;
using SharedKernel.Providers;
using SharedKernel.RabbitMQ;
using SharedKernel.Runtime;
using SharedKernel.Runtime.Exceptions;
using SharedKernel.SignalR;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using Caching;
using FluentValidation;
using Enum = SharedKernel.Application.Enum;
using IExceptionHandler = SharedKernel.Runtime.IExceptionHandler;
using static SharedKernel.Application.Enum;

namespace SharedKernel.Configure
{
    public static partial class ConfigureExtension
    {
        #region DependencyInjection
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

        public static IServiceCollection AddCoreRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var appSettingConfiguration = sp.GetRequiredService<IApplicationConfiguration>();
                var rabbitConfig = appSettingConfiguration.GetConfiguration<Rabbit>();
                return new ConnectionFactory
                {
                    HostName = rabbitConfig.Host,
                    UserName = rabbitConfig.Username,
                    Password = rabbitConfig.Password,
                    VirtualHost = rabbitConfig.VirtualHost,
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                };
            });
            services.AddScoped<IRabbitMqClientBase, RabbitMqClientBase>();

            return services;
        }

        public static IServiceCollection AddCoreAuthentication(this IServiceCollection services, IConfiguration Configuration)
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:JwtSettings:Key"])),
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
        

        public static IServiceCollection AddCoreProviders(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddSingleton<IDistributedCacheUserProvider, DistributedCacheUserProvider>();
            services.AddScoped<IS3StorageProvider, S3StorageProvider>();
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

        public static IServiceCollection AddCoreBehaviors(this IServiceCollection services)
        {
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestBehavior<,>));
            return services;
        }
        #endregion
    }
}
