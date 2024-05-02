using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryNavigationQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryNavigationQuery, IList<CategoryNavigationDto>>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    
    public GetCategoryNavigationQueryHandler(
        IMapper mapper,
        ICategoryReadOnlyRepository categoryReadOnlyRepository
        ) : base(mapper)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
    }

    public async Task<IList<CategoryNavigationDto>> Handle(GetCategoryNavigationQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryReadOnlyRepository.GetCategoryNavigationAsync(cancellationToken);
        return result;
    }
}