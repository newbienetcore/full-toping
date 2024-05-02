using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Middlewares;

public class AuthorMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("open-school-author", "Do Hung");
        context.Response.Headers.Add("open-school-facebook", "https://www.facebook.com/...");
        context.Response.Headers.Add("open-school-email", "devbe2002@gmail.com");
        context.Response.Headers.Add("open-school-contact", "0346109314");

        await _next(context);
    }
}

public static class AuthorMiddlewareMiddlewareExtension
{
    public static IApplicationBuilder UseCoreAuthor(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthorMiddleware>();
    }
}