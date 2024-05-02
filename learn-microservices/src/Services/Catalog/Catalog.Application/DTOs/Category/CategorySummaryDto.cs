using AutoMapper;
using Catalog.Application.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.DTOs;

public class CategorySummaryDto : IMapFrom<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public int OrderNumber { get; set; }
    public int Level { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsSelected { get; set; }

    public CategorySummaryDto Parent { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CategorySummaryDto, Category>().ReverseMap();
    }
}