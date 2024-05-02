using Application.Helpers.Extensions; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Core.Models;
using OnlineShop.Infrastructure;
using OnlineShop.WebApi.Helpers.Extensions;
using OnlineShop.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
#region Services
services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

services.AddEndpointsApiExplorer();
services.AddCors(options =>
{
    options.AddPolicy("AllowAll", x => x.AllowAnyHeader().
                                        AllowAnyOrigin().
                                        AllowAnyMethod());
});

services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.Parse("8.0.23-mysql"),
        mySqlOptions =>
        {
            mySqlOptions.MigrationsAssembly("OnlineShop.WebApi");
        });
}, ServiceLifetime.Singleton);

services.SwaggerConfig();
services.JwtConfig(builder.Configuration);
services.AutoMapperConfig();
services.AddMemoryCache();

#endregion
var app = builder.Build();
#region App
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Base Core V3.1");
    });
}
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseHttpsRedirection();
app.RouterDefinition();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.Run();
#endregion