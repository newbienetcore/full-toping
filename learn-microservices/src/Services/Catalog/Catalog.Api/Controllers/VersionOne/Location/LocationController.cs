using Catalog.Application.Features.VersionOne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class LocationController : BaseController
{
    [AllowAnonymous]
    [HttpGet("provinces")]
    public async Task<IActionResult> GetAllProvincesAsync(CancellationToken cancellationToken = default)
    {
        var provinces = await Mediator.Send(new GetAllProvincesQuery(), cancellationToken);
        return Ok(new ApiSimpleResult(provinces));
    }
    
    // [AllowAnonymous]
    // [HttpGet("provinces/{id:guid}/districts")]
    // public async Task<IActionResult> GetAllDistrictByProvinceIdAsync([FromRoute(Name = "id")]long provinceId, CancellationToken cancellationToken = default)
    // {
    //     var provinces = await Mediator.Send(new GetAllDistrictsByProvinceIdQuery(provinceId), cancellationToken);
    //     return Ok(new ApiSimpleResult(provinces));
    // }
    //
    // [AllowAnonymous]
    // [HttpGet("districts/{id:guid}/wards")]
    // public async Task<IActionResult> GetAllWardsByDistrictIdAsync([FromRoute(Name = "id")]long districtId, CancellationToken cancellationToken = default)
    // {
    //     var provinces = await Mediator.Send(new GetAllWardsByDistrictIdQuery(districtId), cancellationToken);
    //     return Ok(new ApiSimpleResult(provinces));
    // }
    
    [AllowAnonymous]
    [HttpGet("provinces/{code}/districts")]
    public async Task<IActionResult> GetAllDistrictByProvinceCodeAsync([FromRoute(Name = "code")]string provinceCode, CancellationToken cancellationToken = default)
    {
        var provinces = await Mediator.Send(new GetAllDistrictsByProvinceCodeQuery(provinceCode), cancellationToken);
        return Ok(new ApiSimpleResult(provinces));
    }
    
    [AllowAnonymous]
    [HttpGet("districts/{code}/wards")]
    public async Task<IActionResult> GetAllWardsByDistrictCodeAsync([FromRoute(Name = "code")]string districtCode, CancellationToken cancellationToken = default)
    {
        var provinces = await Mediator.Send(new GetAllWardsByDistrictCodeQuery(districtCode), cancellationToken);
        return Ok(new ApiSimpleResult(provinces));
    }
}