using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllDistrictsByProvinceIdQuery : BaseAllowAnonymousQuery<IList<LocationDistrictDto>>
{
    public long ProvinceId { get; init; }

    public GetAllDistrictsByProvinceIdQuery(long provinceId) => ProvinceId = provinceId;
}