using AutoMapper;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Brands;
using Shared.SeedWork;

namespace Catalog.API.Controllers;

public class BrandController : ApiControllerBase
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;
    public BrandController(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("api/brands")]
    public async Task<IActionResult> GetAllAsync([FromQuery] BrandFilterRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }

    [HttpGet]
    [Route("api/brands/search")]
    public async Task<IActionResult> GetPagedListAsync([FromQuery] BrandFilterRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }

    [HttpGet]
    [Route("api/brands/{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] Guid brandId,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }

    [HttpPost]
    [Route("api/brands")]
    public async Task<IActionResult> CreateAsync([FromBody] EditBrandDto editBrandDto,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }

    [HttpPut]
    [Route("api/brands/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")] Guid brandId, EditBrandDto editBrandDto,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }

    [HttpDelete]
    [Route("api/brands/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] Guid brandId,
        CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResponse());
    }
}