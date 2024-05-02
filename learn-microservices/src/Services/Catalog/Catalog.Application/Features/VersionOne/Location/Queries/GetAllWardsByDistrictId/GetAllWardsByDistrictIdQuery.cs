using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;


public class GetAllWardsByDistrictIdQuery : BaseAllowAnonymousQuery<IList<LocationWardDto>>
{
    public long DistrictId { get; init; }

    public GetAllWardsByDistrictIdQuery(long districtId) => DistrictId = districtId;
}