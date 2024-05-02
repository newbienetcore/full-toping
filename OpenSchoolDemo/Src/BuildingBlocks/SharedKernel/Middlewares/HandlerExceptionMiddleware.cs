using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Contracts;
using SharedKernel.Libraries;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Middlewares;

public class HandlerExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HandlerExceptionMiddleware> _logger;

    public HandlerExceptionMiddleware(RequestDelegate next, ILogger<HandlerExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ApiResult();
        var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();

         if (exception is CatchableException)
        {
            response.Error = new Error(500, exception.Message, "CATCHABLE");
        }
        else if (exception is ForbiddenException)
        {
            response.Error = new Error(403, localizer["not_permission"].Value, "NOT_PERMISSION");
        }
        else if (exception is SqlInjectionException)
        {
            response.Error = new Error(400, Secure.MsgDetectedSqlInjection, "SQL_INJECTION");
        }
        else if (exception is BadRequestException badRequestException)
        {
            if (badRequestException.Body != null)
            {
                response = new ApiSimpleResult
                {
                    Data = badRequestException.Body,
                    Error = new Error(400, exception.Message, badRequestException.Type)
                };
            }
            else
            {
                response.Error = new Error(400, exception.Message, badRequestException.Type);
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

            response.Error = new Error(
                HttpStatusCode.BadRequest,
                JsonConvert.SerializeObject(validateResult),
                "BAD_REQUEST");
        }
        // Unknown exception
        else
        {
            response.Error = new Error(500, localizer["system_error_occurred"].Value, "SYSTEM_ERROR_OCCURRED");
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }));
    }
}

public static class HandlerExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseCoreHandlerException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnauthorizedHandlerMiddleware>();
    }
}