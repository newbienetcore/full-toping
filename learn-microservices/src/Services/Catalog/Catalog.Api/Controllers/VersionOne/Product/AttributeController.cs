using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class AttributeController : BaseController
{
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetAttributeAllQuery(), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("get-by-id/{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")]Guid attributeId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetAttributeByIdQuery(attributeId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    // [AllowAnonymous]
    // [HttpPost("paging")]
    // public async Task<IActionResult> GetPagingAsync([FromBody]PagingRequest pagingRequest, CancellationToken cancellationToken = default)
    // {
    //     // var result = await Mediator.Send(new GetSupplierPagingQuery(pagingRequest), cancellationToken);
    //     return Ok(new ApiSimpleResult(default!));
    // }
    
    [AllowAnonymous]
    [HttpGet("create")]
    public async Task<IActionResult> CreateAsync(CreateAttributeCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")]Guid attributeId, [FromBody]UpdateAttributeCommand command, CancellationToken cancellationToken = default)
    {
        if (attributeId != command.Id)
        {
            return BadRequest();
        }
        
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")]Guid attributeId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new DeleteAttributeCommand(attributeId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
}