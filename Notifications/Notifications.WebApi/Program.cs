using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Infrastructure.Persistence;
using Notifications.WebApi;
using Notifications.WebApi.Middlewares;
using Serilog;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start server Web API IChiba.Notification.");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Host.AddAppConfigurations();
    
    builder.Services.AddWebApiServices(builder.Configuration, builder.Environment); ;
    
    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var applicationDbContextSeed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
        await applicationDbContextSeed.InitialiseAsync();
        await applicationDbContextSeed.SeedAsync();
        var a = await applicationDbContextSeed.GetAllAsync();
        var b = "";
    }
    

    app.UseExceptionMiddleware();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
    
    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Unhandled exception");
}
finally
{
    Log.Information("Shut dow web api Web API IChiba.Notification complete");
    Log.CloseAndFlush();
}
