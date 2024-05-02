using AutoMapper;
using Catalog.Application.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs;

public class CategoryDto : IMapFrom<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Alias  { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    public int OrderNumber { get; set; }
    public bool Status { get; set; }
    public string Path { get; set; }
    public Guid? ParentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CategoryDto, Category>().ReverseMap();
    }
}