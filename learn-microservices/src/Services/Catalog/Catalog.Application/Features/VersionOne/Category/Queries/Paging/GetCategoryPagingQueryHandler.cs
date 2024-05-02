using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryPagingQuery, IPagedList<CategoryDto>>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IFileService _fileService;
    public GetCategoryPagingQueryHandler(
        IMapper mapper,
        ICategoryReadOnlyRepository categoryReadOnlyRepository, 
        IFileService fileService
        ) : base(mapper)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _fileService = fileService;
    }

    public async Task<IPagedList<CategoryDto>> Handle(GetCategoryPagingQuery request, CancellationToken cancellationToken)
    {
        var paging = await _categoryReadOnlyRepository.GetPagingResultAsync(request.PagingRequest, cancellationToken);
        foreach (var item in paging.Items)
        {
            paging.Items.First(x => x.FileName.Equals(item.FileName)).Url = _fileService.GetFileUrl(item.FileName);
            
            // Handle caching
        }
        
        return paging;
    }
}