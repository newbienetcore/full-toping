using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne.Caching;

[ApiVersion("1.0")]
public class CachingController : BaseController
{
    [HttpPost("clear-all")]
    public async Task<IActionResult> ClearAllCaching(CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new ClearAllCachingCommand(), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
}