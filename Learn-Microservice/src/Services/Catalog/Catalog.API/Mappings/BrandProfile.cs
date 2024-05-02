using AutoMapper;
using Catalog.API.Entities;
using Contracts.Common.Interfaces;
using Shared.DTOs.Brands;
using Shared.DTOs.PagedList;

namespace Catalog.API.Mappings;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>().ReverseMap();
        
    }
}