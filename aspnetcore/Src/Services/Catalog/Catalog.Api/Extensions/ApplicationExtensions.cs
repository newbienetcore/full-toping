using Catalog.Api.Repositories;
using Catalog.Api.Repositories.Interfaces;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Middlewares;

namespace Catalog.Api.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        // app.UseHttpsRedirection(); for production only

        app.UseAuthorization();

        app.UseCoreHandlerException();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
    
}