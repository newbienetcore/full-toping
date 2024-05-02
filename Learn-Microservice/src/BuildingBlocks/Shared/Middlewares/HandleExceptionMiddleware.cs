using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.SeedWork;
using ApplicationException = Shared.Exceptions.ApplicationException;

namespace Shared.Middlewares;

public class HandleExceptionMiddleware : IMiddleware
{
    private readonly ILogger<HandleExceptionMiddleware> _logger;

    public HandleExceptionMiddleware(ILogger<HandleExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
            _logger.LogError(ex, ex.Message);
        }
    }

    #region [PRIVATE METHOD]

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ApiResponseBase();

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