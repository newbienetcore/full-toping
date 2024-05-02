using Catalog.Application.Mappings;
using Catalog.Domain.Entities;
using SharedKernel.Domain;

namespace Catalog.Application.DTOs;

public class LocationDistrictDto : IMapFrom<LocationDistrict>
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public LocationType Type { get; set; }
}