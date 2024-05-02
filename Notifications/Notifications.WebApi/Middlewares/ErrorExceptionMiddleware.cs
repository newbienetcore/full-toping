using System.Net;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notifications.Application.Common.Models.Abstractions;
using Notifications.Application.Common.Models.Responses;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.WebApi.Middlewares;

public class ErrorExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorExceptionMiddleware> _logger;

    public ErrorExceptionMiddleware(RequestDelegate next, ILogger<ErrorExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
            _logger.LogError(ex.Message);
        }
    }

    #region [PRIVATE METHOD]

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ApiResponseBase response = new ApiResponseBase();

        switch (exception)
        {
            case ValidationException validationException:
            {
                response.Error = validationException.Errors.GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToList());
                response.StatusCode = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            }
            case ApplicationException applicationException:
            {
                response.ErrorKey = applicationException.ErrorKey;
                response.StatusCode = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            }
            default:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
        }

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    #endregion
}
