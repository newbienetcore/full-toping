using AutoMapper;
using Catalog.API.Entities;
using Contracts.Common.Interfaces;
using Shared.DTOs.Categories;
using Shared.DTOs.PagedList;
using Shared.DTOs.Products;

namespace Catalog.API.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        
    }
}