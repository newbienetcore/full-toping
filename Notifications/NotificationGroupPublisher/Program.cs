

using NotificationGroupPublisher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebApiServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger((Swashbuckle.AspNetCore.Swagger.SwaggerOptions options) =>
{
    // Cấu hình Swagger
});
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();