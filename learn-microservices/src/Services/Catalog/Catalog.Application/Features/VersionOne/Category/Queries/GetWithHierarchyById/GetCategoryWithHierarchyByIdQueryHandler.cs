using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryWithHierarchyByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryWithHierarchyByIdQuery, IList<CategorySummaryDto>>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IFileService _fileService;
    public GetCategoryWithHierarchyByIdQueryHandler(
        IMapper mapper,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IStringLocalizer<Resources> localizer,
        IFileService fileService
        ) : base(mapper)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _fileService = fileService;
        _localizer = localizer;
    }

    public async Task<IList<CategorySummaryDto>> Handle(GetCategoryWithHierarchyByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryReadOnlyRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new BadRequestException(_localizer["category_does_not_exist_or_was_deleted"].Value);
        }
        
        var categories = await _categoryReadOnlyRepository.GetCategoryHierarchyAsync(category, cancellationToken);
        foreach (var item in categories)
        {
            categories.First(x => x.FileName.Equals(item.FileName)).Url = _fileService.GetFileUrl(item.FileName);
            
            // Handle caching
        }
        
        return categories;
    }
}