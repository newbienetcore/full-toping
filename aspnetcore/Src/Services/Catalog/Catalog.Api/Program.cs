using Catalog.Api.Extensions;
using Catalog.Api.Persistence;
using Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");


try
{
    var services = builder.Services;
    var configuration = builder.Configuration;
    
    builder.Host.UseCoreSerilog();
    
    builder.Host.AddAppConfiguration();
    services.AddInfrastructure(configuration);

    var app = builder.Build();

    app.UseInfrastructure();
    
    app.MigrateDatabase<CatalogDbContext>((context, _) =>
        {
            CatalogDbContextSeed.SeedProductAsync(context, Log.Logger).Wait();
        })
        .Run();
    
    app.Run();
    
    
}
catch (Exception ex) 
    when(!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Catalog Api complete.");
    Log.CloseAndFlush();
}