using System.Linq.Expressions;
using AutoMapper;
using Catalog.API.Constants;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Categories;
using Shared.SeedWork;
using ApplicationException = Shared.Exceptions.ApplicationException;

namespace Catalog.API.Controllers;

public class CategoryController : ApiControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    [Route("api/categories")]
    public async Task<IActionResult> GetAllAsync([FromQuery] CategoryFilterRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.FindAllAsync(
            conditionPredicates: new List<(bool Condition, Expression<Func<Category, bool>> Predicate)>()
            {
                (Condition: !string.IsNullOrWhiteSpace(filter.Keyword),
                    Predicate: c => c.Name.Contains(filter.Keyword) || (string.IsNullOrWhiteSpace(c.Description) || c.Description.Contains(filter.Keyword))),
                (Condition: filter.CategoryId != Guid.Empty, Predicate: c => c.Id == filter.CategoryId)
            },
            orderBy: q => q.OrderByDescending(c => c.CreateDate),
            cancellationToken: cancellationToken);
        
        return Ok(new ApiResponse<IEnumerable<CategoryDto>>(_mapper.Map<IEnumerable<CategoryDto>>(categories)));
    }


    [HttpGet]
    [Route("api/categories/search")]
    public async Task<IActionResult> GetPagedListAsync([FromQuery] CategoryFilterRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.FindAllAsync(
            conditionPredicates: new List<(bool Condition, Expression<Func<Category, bool>> Predicate)>()
            {
                (Condition: !string.IsNullOrWhiteSpace(filter.Keyword), Predicate: c => c.Name.Contains(filter.Keyword) || (string.IsNullOrWhiteSpace(c.Description) || c.Description.Contains(filter.Keyword))),
                (Condition: filter.CategoryId != Guid.Empty, Predicate: c => c.Id == filter.CategoryId)
            },
            orderBy: q => q.OrderByDescending(c => c.CreateDate),
            cancellationToken: cancellationToken
            );

        return Ok(new ApiResponse<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(categories)));
    }

    [HttpGet]
    [Route("api/categories/{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(categoryId,
                cancellationToken: cancellationToken);

        if (category is null) throw new ApplicationException(ErrorCode.CategoryNotFound, ErrorCode.CategoryNotFound);

        return Ok(new ApiResponse<CategoryDto>(_mapper.Map<CategoryDto>(category)));
    }

    [HttpPost]
    [Route("api/categories")]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var duplicate = await _categoryRepository.GetAsync(
            predicate: c => c.Name == categoryDto.Name, cancellationToken: cancellationToken);
        if (duplicate is not null)
            throw new ApplicationException(ErrorCode.CategoryDuplicate, ErrorCode.CategoryDuplicate);
        
        var categoryNew = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.InsertAsync(categoryNew, cancellationToken);
        return Ok(new ApiResponse());
    }

    [HttpPut]
    [Route("api/categories/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid categoryId, [FromBody] CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetAsync(predicate: c => c.Id == categoryId,
                cancellationToken: cancellationToken);

        if (category is null) 
            throw new ApplicationException(ErrorCode.CategoryNotFound, ErrorCode.CategoryNotFound);

        var duplicate = await _categoryRepository.GetAsync(
            predicate: c => c.Name == categoryDto.Name && c.Id == categoryId, cancellationToken: cancellationToken);
        if (duplicate is not null)
            throw new ApplicationException(ErrorCode.CategoryDuplicate, ErrorCode.CategoryDuplicate);

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Ok(new ApiResponse());
    }

    [HttpDelete]
    [Route("api/categories/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetAsync(predicate: c => c.Id == categoryId,
            cancellationToken: cancellationToken);

        if (category is null) throw new ApplicationException(ErrorCode.CategoryNotFound, ErrorCode.CategoryNotFound);

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        
        return Ok(new ApiResponse());
    }
}