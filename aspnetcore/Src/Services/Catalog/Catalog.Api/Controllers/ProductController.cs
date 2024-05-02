using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Properties;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SharedKernel.Contracts;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;

    public ProductController(
        IProductRepository repository,
        IStringLocalizer<Resources> localizer,
        IMapper mapper
        )
    {
        _repository = repository;
        _localizer = localizer;
        _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetProductsAsync(cancellationToken);
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(new ApiSimpleResult(products));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetProductByIdAsync(id, cancellationToken);
        if (result == null)
        {
            throw new BadRequestException(_localizer["product_does_not_exist_or_was_deleted"].Value);
        }
        
        return Ok(new ApiSimpleResult(_mapper.Map<ProductDto>(result)));
    }

    [HttpGet("get-by-no/{no}")]
    public async Task<IActionResult> GetByNoAsync([FromRoute] string no, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetProductByNoAsync(no, cancellationToken);
        if (result == null)
        {
            throw new BadRequestException(_localizer["product_does_not_exist_or_was_deleted"].Value);
        }
        
        return Ok(new ApiSimpleResult(_mapper.Map<ProductDto>(result)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetProductByNoAsync(productDto.No, cancellationToken);
        if (product != null) throw new BadRequestException($"Product No: {productDto.No} is existed.");
        
        product = _mapper.Map<Product>(productDto);
        
        await _repository.CreateProductAsync(product, cancellationToken);
        await _repository.UnitOfWork.CommitAsync(cancellationToken);
        
        var result = _mapper.Map<ProductDto>(product);
        return Ok(new ApiSimpleResult(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([Required] Guid id, [FromBody] UpdateProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new BadRequestException(_localizer["product_does_not_exist_or_was_deleted"].Value);
        }
        
        _mapper.Map(productDto, product);
        
        await _repository.UpdateProductAsync(product, cancellationToken);
        await _repository.UnitOfWork.CommitAsync(cancellationToken);
        
        var result = _mapper.Map<ProductDto>(product);

        return Ok(new ApiSimpleResult(result));

    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([Required] Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new BadRequestException(_localizer["product_does_not_exist_or_was_deleted"].Value);
        }

        await _repository.DeleteProductAsync(id, cancellationToken);
        await _repository.UnitOfWork.CommitAsync(cancellationToken);

        return Ok(new ApiSimpleResult(id));
    }
    
}