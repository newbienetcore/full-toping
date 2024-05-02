using Catalog.API;
using Catalog.API.Extensions;
using Catalog.API.Persistence;
using Common.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Product Api up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    
    builder.Host.AddAppConfigurations();
    
    builder.Services.AddServices(builder.Configuration, builder.Environment);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => 
            options.SwaggerEndpoint("./v1/swagger.json", 
                $"Swagger {builder.Environment.EnvironmentName} API v1"));
    }
    
    app.AddApplication();

    app.MigrateDatabase<ApplicationDbContext>((context, _) =>
    {
        ApplicationDbContextSeed.SeedAsync(context, Log.Logger).Wait();
        
    }).Run();

}
catch (Exception exception)
{
    string type = exception.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(exception, $"Unhandled exception {exception.Message}");
}
finally
{
    Log.Information("Shut down Product Api complete.");
    Log.CloseAndFlush();
}