using System.Reflection;
using Basket.API;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);


Log.Information($"Start {builder.Environment.EnvironmentName} up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSetting(builder.Configuration);
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    
    // Add services to the container.
    builder.Services.AddServices();
    builder.Services.AddRedisServices(builder.Configuration);
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    
    // Configure Masstransit
    builder.Services.ConfigureMassTransit(builder.Configuration);
    
    builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => 
            options.SwaggerEndpoint("./v1/swagger.json", 
                $"Swagger {builder.Environment.EnvironmentName} API v1"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.Run();
    
    Log.Information($"Start {builder.Environment.EnvironmentName} up");
}
catch(Exception exception)
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
    Log.Information($"Shut down  {builder.Environment.EnvironmentName} complete.");
    Log.CloseAndFlush();
}