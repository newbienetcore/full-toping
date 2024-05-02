using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;
using SharedKernel.Libraries;
using Enum = SharedKernel.Application.Enum;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class CategoryController : BaseController
{
    [AllowAnonymous]
    [Authorize]
    [AuthorizationRequest(new Enum.ActionExponent[] { Enum.ActionExponent.Add })]
    [HttpGet("get-navigation")]
    public async Task<IActionResult> GetCategoryNavigationAsync(CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetCategoryNavigationQuery(), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("get-hierarchy-by-id/{id}")]
    public async Task<IActionResult> GetCategoryWithHierarchyByIdAsync([FromRoute(Name = "id")]Guid categoryId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetCategoryWithHierarchyByIdQuery(categoryId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")]Guid categoryId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(categoryId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("get-by-alias/{alias}")]
    public async Task<IActionResult> GetByAliasAsync([FromRoute(Name = "alias")]string alias, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetCategoryByAliasQuery(alias), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPost("get-paging")]
    public async Task<IActionResult> GetPagingAsync([FromBody]PagingRequest pagingRequest, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetCategoryPagingQuery(pagingRequest), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody]CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")]Guid categoryId, [FromBody]UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (categoryId != command.Id)
        {
            return BadRequest();
        }
        
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")]string categoryId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(categoryId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
}