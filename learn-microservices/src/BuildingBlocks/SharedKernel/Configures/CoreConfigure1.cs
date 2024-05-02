using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application;
using SharedKernel.Libraries;
using SharedKernel.Middlewares;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;
using System.Globalization;
using System.Net;
using FluentValidation;
using static SharedKernel.Application.Enum;

namespace SharedKernel.Configure;

public static partial class ConfigureExtension
{
    #region Middlewares

    public static void UseCoreConfigure(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCoreAuthor();
        app.UseCoreLocalization();
        if (!environment.IsDevelopment())
        {
            app.UseReject3P();
        }

        app.UseCoreExceptionHandler();
        app.UseIpRateLimiting();
        app.UseForwardedHeaders();
        // app.UseHttpsRedirection(); 
        app.UseCoreUnauthorized();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        });
        // app.UseCoreHealthChecks();
    }

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


    public static void UseCoreExceptionHandler(this IApplicationBuilder app)
    {
        // Handle exception
        app.UseExceptionHandler(a => a.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            var responseContent = new ApiResult();
            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();

            // Catchable
            if (exception is CatchableException)
            {
                responseContent.Error = new Error(500, exception.Message, "");
            }
            else if (exception is ForbiddenException)
            {
                responseContent.Error = new Error(403, localizer["not_permission"].Value);
            }
            else if (exception is SqlInjectionException)
            {
                responseContent.Error = new Error(400, Secure.MsgDetectedSqlInjection);
            }
            else if (exception is BadRequestException bRException)
            {
                if (bRException.Body is not null)
                {
                    responseContent = new ApiSimpleResult
                    {
                        Data = bRException.Body,
                        Error = new Error(400, exception.Message, bRException.Type)
                    };
                }
                else
                {
                    responseContent.Error = new Error(400, exception.Message, bRException.Type);
                }
            }
            else if (exception is ValidationException vException)
            {
                var errors = vException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => string.Join("; ", x.Select(y => y.ErrorMessage))
                    );

                var validateResult = new ValidateResult();

                foreach (var error in errors)
                {
                    var validateField = new ValidateField
                    {
                        FieldName = error.Key,
                        Code = ValidateCode.Required,
                        ErrorMessage = error.Value
                    };

                    validateResult.ValidateFields.Add(validateField);
                }

                responseContent.Error = new Error(
                    HttpStatusCode.BadRequest,
                    JsonConvert.SerializeObject(validateResult),
                    "BAD_REQUEST");
            }
            // Unknown exception
            else
            {
                responseContent.Error = new Error(500, localizer["system_error_occurred"].Value);
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }));
    }

    #endregion
}