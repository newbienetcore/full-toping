using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class CloudController : BaseController
{
    [DisableRequestSizeLimit]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync([FromForm]UploadCloudFileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [DisableRequestSizeLimit]
    [HttpPost("uploads")]
    public async Task<IActionResult> UploadFileMultipleAsync([FromForm]List<IFormFile> files, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new UploadMultipleCloudFileCommand(files), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
}