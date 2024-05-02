using Microsoft.AspNetCore.Builder;

namespace Notifications.WebApi.Middlewares;

public static class UseMiddleware
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorExceptionMiddleware>();
    }
}