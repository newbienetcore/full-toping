using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetAllWardsByDistrictIdQueryHandler : BaseQueryHandler, IRequestHandler<GetAllWardsByDistrictIdQuery, IList<LocationWardDto>>
{
    private readonly ILocationReadOnlyRepository _locationReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public GetAllWardsByDistrictIdQueryHandler(
        IMapper mapper,
        ILocationReadOnlyRepository locationReadOnlyRepository,
        IStringLocalizer<Resources> localizer
        ) : base(mapper)
    {
        _locationReadOnlyRepository = locationReadOnlyRepository;
        _localizer = localizer;
    }

    public async Task<IList<LocationWardDto>> Handle(GetAllWardsByDistrictIdQuery request, CancellationToken cancellationToken)
    {
        var district = await _locationReadOnlyRepository.GetDistrictByIdAsync(request.DistrictId, cancellationToken);
        if (district == null)
        {
            throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"].Value);
        }
        
        return await _locationReadOnlyRepository.GetAllWardsByDistrictIdAsync(request.DistrictId, cancellationToken);
    }
}