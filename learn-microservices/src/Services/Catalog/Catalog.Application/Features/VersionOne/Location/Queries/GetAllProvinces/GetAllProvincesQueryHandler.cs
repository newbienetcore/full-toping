using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAllProvincesQueryHandler : BaseQueryHandler, IRequestHandler<GetAllProvincesQuery, IList<LocationProvinceDto>> 
{
    private readonly ILocationReadOnlyRepository _locationReadOnlyRepository;
    public GetAllProvincesQueryHandler(
        IMapper mapper,
        ILocationReadOnlyRepository locationReadOnlyRepository
    ) : base(mapper)
    {
        _locationReadOnlyRepository = locationReadOnlyRepository;
    }

    public async Task<IList<LocationProvinceDto>> Handle(GetAllProvincesQuery request, CancellationToken cancellationToken)
    {
        return await _locationReadOnlyRepository.GetAllProvincesAsync(cancellationToken);
    }
}