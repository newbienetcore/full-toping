using AutoMapper;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using SharedKernel.Libraries;

namespace Catalog.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .IgnoreAllNonExisting();
    }
}