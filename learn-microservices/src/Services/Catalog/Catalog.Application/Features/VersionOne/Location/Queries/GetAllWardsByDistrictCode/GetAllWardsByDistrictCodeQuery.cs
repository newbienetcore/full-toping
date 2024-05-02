using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllWardsByDistrictCodeQuery : BaseAllowAnonymousQuery<IList<LocationWardDto>>
{
    public string DistrictCode { get; init; }

    public GetAllWardsByDistrictCodeQuery(string districtCode) => DistrictCode = districtCode;
}