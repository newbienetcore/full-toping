using Caching;
using Identity.Api.Configures;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.HttpOverrides;
using SharedKernel.Configures;
using SharedKernel.Core;
using SharedKernel.Filters;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

try
{
    builder.AddCoreWebApplication();
    
    // Core setting project
    CoreSettings.SetJWTConfig(configuration);
    CoreSettings.SetConnectionStrings(configuration);
    
    // Services
    services.AddCoreServices(configuration);
    
    services.AddCoreAuthentication(builder.Configuration);
    
    services.AddCoreCaching(builder.Configuration);
    
    services.AddHealthChecks();
    
    services.Configure<ForwardedHeadersOptions>(o => o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
    
    services.AddCurrentUser();
    
    services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AccessTokenValidatorAsyncFilter());
    });
    
    services.AddInfrastructureServices(configuration);

    services.AddApplicationServices(configuration);
    
    // Configure the HTTP request pipeline.
    var app = builder.Build();

    // Pipelines
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerVersioning();
    }
    
    app.UseCoreCors(configuration);

    app.UseCoreWebApplication(app.Environment);
    
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var contextSeed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
        await contextSeed.InitialiseAsync();
        await contextSeed.SeedAsync();
        await contextSeed.SyncPermissionsBasedOnChanges();
    }
    
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shutdown {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}