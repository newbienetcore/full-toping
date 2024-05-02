using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Authorization.Events;
using Shared.Utilities;

namespace Shared.Authorization.Extensions;

public static class JwtAuthorizationExtension
{
    public static IServiceCollection AddJwtBearer(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<JwtBearerOptions> configureOptions = null)
    {
        var jwtSetting = configuration.GetOptions<JwtSetting>(JwtSetting.SettingSection);
        if (jwtSetting.Key is null)
            throw new ArgumentNullException("JwtSetting.Key is null or empty");
        if(jwtSetting.Issuer is null)
            throw new ArgumentNullException("JwtSetting.Issuer is null or empty");
        if(jwtSetting.Audience is null)
            throw new ArgumentNullException("JwtSetting.Audience is null or empty");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSetting.Issuer,
                    ValidAudience = jwtSetting.Audience
                };
                options.Events = new JwtBearerDefaultEvent();
                configureOptions?.Invoke(options);
            });
        
        return services;
    }
}