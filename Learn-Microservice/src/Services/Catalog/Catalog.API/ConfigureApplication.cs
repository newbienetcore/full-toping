
using Shared.Middlewares;

namespace Catalog.API;

public static class ConfigureApplication
{
    public static void AddApplication(this IApplicationBuilder app)
    {
        
        app.UseStaticFiles();
        
        app.UseMiddleware<HandleExceptionMiddleware>();
        
        app.UseRouting();
        // app.UseHttpsRedirection(); // for production only

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}