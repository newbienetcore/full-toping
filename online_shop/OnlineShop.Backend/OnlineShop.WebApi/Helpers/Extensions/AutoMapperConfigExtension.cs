
using OnlineShop.WebApi.Helpers.DataMapping;

namespace OnlineShop.WebApi.Helpers.Extensions
{
    public static class AutoMapperConfigExtension
    {
        public static IServiceCollection AutoMapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CreateUserMapping));
            return services;
        }
    }
}
