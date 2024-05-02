
using OnlineShop.Application.ViewModels;

namespace OnlineShop.Application.Helpers
{
    public class MappingConfig
    {
        public static void AutoMapperConfig(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
