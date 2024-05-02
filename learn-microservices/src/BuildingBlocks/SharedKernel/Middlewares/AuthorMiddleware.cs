using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Middlewares
{
    public class AuthorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("learn-microservices-author", "Do Chi Hung");
            context.Response.Headers.Add("learn-microservices-facebook", "https://www.facebook.com/dohungiy/");
            context.Response.Headers.Add("learn-microservices-email", "dohung.csharp@gmail.com");
            context.Response.Headers.Add("learn-microservices-contact", "0976580418");

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
}
