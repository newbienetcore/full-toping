using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllDistrictsByProvinceCodeQuery : BaseAllowAnonymousQuery<IList<LocationDistrictDto>>
{
    public string ProvinceCode { get; init; }

    public GetAllDistrictsByProvinceCodeQuery(string provinceCode) => ProvinceCode = provinceCode;
}