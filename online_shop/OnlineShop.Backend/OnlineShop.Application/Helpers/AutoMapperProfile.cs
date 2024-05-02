using AutoMapper;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Schemas;

namespace OnlineShop.Application.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<ProductSchema, ProductVM>().ReverseMap();
			CreateMap<OrderSchema, CustomerPlaceOrder>().ReverseMap();
			CreateMap<CustomerSchema, RegisterVM>().ReverseMap();
        }
	}
}
