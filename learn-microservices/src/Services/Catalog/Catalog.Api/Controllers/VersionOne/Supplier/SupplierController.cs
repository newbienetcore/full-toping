using System.ComponentModel.DataAnnotations;
using Catalog.Application.DTOs;
using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class SupplierController : BaseController
{
    [AllowAnonymous]
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")]string supplierId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetSupplierByIdQuery(supplierId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpGet("get-by-alias/{alias}")]
    public async Task<IActionResult> GetByAliasAsync([FromRoute(Name = "alias")]string alias, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetSupplierByAliasQuery(alias), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPost("paging")]
    public async Task<IActionResult> GetPagingAsync([FromBody]PagingRequest pagingRequest, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetSupplierPagingQuery(pagingRequest), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody]CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")]Guid supplierId, [FromBody]UpdateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        if (supplierId != command.Id)
        {
            return BadRequest();
        }
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")]string supplierId, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new DeleteSupplierCommand(supplierId), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
    [AllowAnonymous]
    [HttpDelete("delete-multiple")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody]IList<Guid> supplierIds, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new DeleteMultipleSupplierCommand(supplierIds), cancellationToken);
        return Ok(new ApiSimpleResult(result));
    }
    
}