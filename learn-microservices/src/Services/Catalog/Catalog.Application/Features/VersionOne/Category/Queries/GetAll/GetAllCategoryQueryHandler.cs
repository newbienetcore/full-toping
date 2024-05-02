using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllCategoryQueryHandler : BaseQueryHandler, IRequestHandler<GetAllCategoryQuery, IList<CategoryDto>>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    public GetAllCategoryQueryHandler(
        IMapper mapper,
        ICategoryReadOnlyRepository categoryReadOnlyRepository
        ) : base(mapper)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
    }

    public async Task<IList<CategoryDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _categoryReadOnlyRepository.GetAllCategoryAsync(cancellationToken);
    }
}