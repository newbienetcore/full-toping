using AspNetCoreRateLimit;
using SharedKernel.Configures;
using SharedKernel.Middlewares;

namespace Identity.Api.Configures;

public static class ConfigureApplications
{
    public static void UseCoreWebApplication(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCoreLocalization();
        if (!environment.IsDevelopment())
        {
            app.UseReject3P();
        }
        app.UseCoreHandlerException();
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
        });
        
        //app.UseCoreHealthChecks();
    }
    public static void UseSwaggerVersioning(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        });
    }
}