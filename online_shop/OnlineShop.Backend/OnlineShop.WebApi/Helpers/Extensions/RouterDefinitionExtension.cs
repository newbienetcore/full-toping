using System.Text;
using AutoMapper;
using OnlineShop.Core.Models;
using OnlineShop.Infrastructure;
using OnlineShop.WebApi.Routers;

namespace OnlineShop.WebApi.Helpers.Extensions
{
    public static class RouterDefinitionExtension
    {
        public static WebApplication RouterDefinition(this WebApplication app)
        {
            string prefix = "api/";
            var configuration = app.Services.GetRequiredService<IConfiguration>();
            string secretKey = configuration["Configuration:SecretKey"];
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            var dbContext = app.Services.GetRequiredService<DataContext>();
            var mapper = app.Services.GetRequiredService<IMapper>();
            List<RouterModel> routers = new ZRouterManager(key, mapper).Get(dbContext);
            foreach (RouterModel router in routers)
            {
                switch (router.Method)
                {
                    case "GET":
                        app.MapGet(prefix + router.Path, router.Action);
                        break;
                    case "POST":
                        app.MapPost(prefix + router.Path, router.Action);
                        break;
                    case "PUT":
                        app.MapPut(prefix + router.Path, router.Action);
                        break;
                    case "DELETE":
                        app.MapDelete(prefix + router.Path, router.Action);
                        break;
                }
            }
            return app;
        }
    }
}
