using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllProvincesQuery : BaseAllowAnonymousQuery<IList<LocationProvinceDto>>
{
    
}