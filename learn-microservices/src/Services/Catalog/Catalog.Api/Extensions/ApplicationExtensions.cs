namespace Catalog.Api.Extensions;

public static class ApplicationExtensions
{
    public static void UseSwaggerVersioning(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.Api version one");
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "Catalog.Api version two");
        });
    }
}