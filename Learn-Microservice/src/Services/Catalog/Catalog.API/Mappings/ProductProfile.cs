using Catalog.API.Entities;
using AutoMapper;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Shared.DTOs.PagedList;
using Shared.DTOs.Products;

namespace Catalog.API.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.ProductImages = src.ProductImages?.Select(i => new ProductImage { ImageUrl = i }).ToList() ?? default!;
            });

        CreateMap<Product, ProductDetailDto>();
        CreateMap<Product, ProductDto>();
    }
}

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        CreateMap<ProductImage, ProductImageDto>().ReverseMap();
    }
}