using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Controllers;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Middlewares;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information("Start Customer Api up");

try
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<HandleExceptionMiddleware>();
    
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString));

    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>))
        .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
        .AddScoped<ICustomerService, CustomerService>();
    
    var app = builder.Build();
    
    app.MapCustomersAPI();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => 
            options.SwaggerEndpoint("./v1/swagger.json", 
                $"Swagger {builder.Environment.EnvironmentName} API v1"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<HandleExceptionMiddleware>();

    app.SeedData().Run();

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
    Log.Information("Shut down Customer Api complete.");
    Log.CloseAndFlush();
}