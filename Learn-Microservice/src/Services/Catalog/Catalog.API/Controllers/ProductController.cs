using System.Linq.Expressions;
using AutoMapper;
using Catalog.API.Constants;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Products;
using Shared.SeedWork;
using ApplicationException = Shared.Exceptions.ApplicationException;

namespace Catalog.API.Controllers;

public class ProductController : ApiControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    [Route("api/products")]
    public async Task<IActionResult> GetAllAsync([FromQuery] ProductFilterRequestDto filter, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.FindAllAsync(
            conditionPredicates: new List<(bool Condition, Expression<Func<Product, bool>> Predicate)>()
            {
                (Condition: !string.IsNullOrWhiteSpace(filter.Keyword), Predicate: c => c.Name.Contains(filter.Keyword) || (string.IsNullOrWhiteSpace(c.Description) || c.Description.Contains(filter.Keyword))),
                (Condition: filter.CategoryId != Guid.Empty, Predicate: c => c.Id == filter.CategoryId),
                (Condition: filter.BrandId != Guid.Empty, Predicate: c => c.Id == filter.BrandId),
                (Condition: filter.FromPrice != null && filter.ToPrice != null && filter.FromPrice <= filter.ToPrice,
                    p => p.Price >= filter.FromPrice && p.Price <= filter.ToPrice)
            },
            orderBy: queryable => queryable.OrderByDescending(p => p.CreateDate),
            cancellationToken: cancellationToken);
        
        return Ok(new ApiResponse<IEnumerable<ProductDto>>(_mapper.Map<IEnumerable<ProductDto>>(products)));
    }
    
    [HttpGet]
    [Route("api/products/search")]
    public async Task<IActionResult> GetPagedListAsync([FromQuery] ProductFilterRequestDto filter, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.FindAllAsync(
            conditionPredicates: new List<(bool Condition, Expression<Func<Product, bool>> Predicate)>()
            {
                (condition: !string.IsNullOrWhiteSpace(filter.Keyword), predicate: c => c.Name.Contains(filter.Keyword) || (string.IsNullOrWhiteSpace(c.Description) || c.Description.Contains(filter.Keyword))),
                (condition: filter.CategoryId != Guid.Empty, predicate: c => c.Id == filter.CategoryId),
                (condition: filter.BrandId != Guid.Empty, predicate: c => c.Id == filter.BrandId),
                (condition: filter.FromPrice != null && filter.ToPrice != null && filter.FromPrice <= filter.ToPrice, predicate: p => p.Price >= filter.FromPrice && p.Price <= filter.ToPrice)
            },
            orderBy: queryable => queryable.OrderByDescending(p => p.CreateDate),
            cancellationToken: cancellationToken
        );

        return Ok(new ApiResponse<List<ProductDto>>(_mapper.Map<List<ProductDto>>(products)));
    }

    [HttpGet]
    [Route("api/products/{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(
            predicate: p => p.Id == productId,
            include: p => p.Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductImages),
            cancellationToken: cancellationToken);

        if (product is null) throw new ApplicationException(ErrorCode.ProductNotFound, ErrorCode.ProductNotFound);

        return Ok(new ApiResponse<ProductDetailDto>(_mapper.Map<ProductDetailDto>(product)));
    }
    
    [HttpPost]
    [Route("api/products")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto createProductDto,
        CancellationToken cancellationToken = default)
    {
        var brand = await _brandRepository.GetAsync(predicate: b => b.Id == createProductDto.BrandId,
            cancellationToken: cancellationToken);
        if (brand is null) throw new ApplicationException(ErrorCode.BrandNotFound, ErrorCode.BrandNotFound);;

        var category = await _categoryRepository.GetAsync(predicate: c => c.Id == createProductDto.CategoryId,
            cancellationToken: cancellationToken);
        if (category is null) throw new ApplicationException(ErrorCode.CategoryNotFound, ErrorCode.CategoryNotFound);;

        var duplicated = await _productRepository.GetAsync(predicate: p => p.Name == createProductDto.Name || p.No == createProductDto.No, cancellationToken: cancellationToken);
        if (duplicated is not null) throw new ApplicationException(ErrorCode.ProductDuplicate, ErrorCode.ProductDuplicate);
        
        var productNew = _mapper.Map<Product>(createProductDto);
        await _productRepository.InsertAsync(productNew, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
        return Ok(new ApiResponse());
    }

    [HttpPut]
    [Route("api/products/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")] Guid productId, [FromBody] UpdateProductDto updateProductDto,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(predicate: p => p.Id == productId, cancellationToken: cancellationToken);
        if (product is null) throw new ApplicationException(ErrorCode.ProductNotFound, ErrorCode.ProductNotFound);

        var brand = await _brandRepository.GetAsync(predicate: b => b.Id == updateProductDto.BrandId,
            cancellationToken: cancellationToken);
        if (brand is null) throw new ApplicationException(ErrorCode.BrandNotFound, ErrorCode.BrandNotFound);

        var category = await _categoryRepository.GetAsync(predicate: c => c.Id == updateProductDto.CategoryId,
            cancellationToken: cancellationToken);
        if (category is null) throw new ApplicationException(ErrorCode.CategoryNotFound, ErrorCode.CategoryNotFound);

        var duplicated = await _productRepository.GetAsync(
            predicate: p => p.Name == updateProductDto.Name  && p.Id != productId, 
            cancellationToken: cancellationToken);
        
        if (duplicated is not null) throw new ApplicationException(ErrorCode.ProductDuplicate, ErrorCode.ProductDuplicate);
        
        product.Name = updateProductDto.Name;
        product.Summary = updateProductDto.Summary;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;
        product.CategoryId = updateProductDto.CategoryId;
        product.BrandId = updateProductDto.BrandId;
            
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);
        
        return Ok(new ApiResponse());
    }

    [HttpDelete]
    [Route("api/products/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(predicate: p => p.Id == productId, cancellationToken: cancellationToken);
        if (product is null) throw new ApplicationException(ErrorCode.ProductNotFound, ErrorCode.ProductNotFound);
        
        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync(cancellationToken);
        
        return Ok(new ApiResponse());
    }

}