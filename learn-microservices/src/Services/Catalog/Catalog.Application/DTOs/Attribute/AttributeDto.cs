using AutoMapper;
using Catalog.Application.Mappings;

namespace Catalog.Application.DTOs;

public class AttributeDto : IMapFrom<Attribute>
{
    public string Key { get; set; }
    public string Value { get; set; }
}