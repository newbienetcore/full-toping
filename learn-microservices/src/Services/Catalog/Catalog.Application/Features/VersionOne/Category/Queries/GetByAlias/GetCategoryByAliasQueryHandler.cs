using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryByAliasQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryByAliasQuery, CategoryDto>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IFileService _fileService;
    private readonly IStringLocalizer<Resources> _localizer;
    public GetCategoryByAliasQueryHandler(
        IMapper mapper, 
        ICategoryReadOnlyRepository categoryReadOnlyRepository, 
        IFileService fileService,
        IStringLocalizer<Resources> localizer
        ) : base(mapper)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _fileService = fileService;
        _localizer = localizer;
    }
    
    public async Task<CategoryDto> Handle(GetCategoryByAliasQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryReadOnlyRepository.GetCategoryByAliasAsync(request.Alias, cancellationToken);
        if (category == null)
        {
            throw new BadRequestException(_localizer["category_does_not_exist_or_was_deleted"].Value);
        }
        
        var categoryDto = _mapper.Map<CategoryDto>(category);
        categoryDto.Url = _fileService.GetFileUrl(categoryDto.Url);
        
        return categoryDto;
    }
}