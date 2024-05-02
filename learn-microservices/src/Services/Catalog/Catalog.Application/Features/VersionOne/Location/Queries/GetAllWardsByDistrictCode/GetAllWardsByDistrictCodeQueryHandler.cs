using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetAllWardsByDistrictCodeQueryHandler : BaseQueryHandler, IRequestHandler<GetAllWardsByDistrictCodeQuery, IList<LocationWardDto>>
{
    private readonly ILocationReadOnlyRepository _locationReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public GetAllWardsByDistrictCodeQueryHandler(
        IMapper mapper,
        ILocationReadOnlyRepository locationReadOnlyRepository,
        IStringLocalizer<Resources> localizer
        ) : base(mapper)
    {
        _locationReadOnlyRepository = locationReadOnlyRepository;
        _localizer = localizer;
    }

    public async Task<IList<LocationWardDto>> Handle(GetAllWardsByDistrictCodeQuery request, CancellationToken cancellationToken)
    {
        var district = await _locationReadOnlyRepository.GetDistrictByCodeAsync(request.DistrictCode, cancellationToken);
        if (district == null)
        {
            throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"].Value);
        }
        
        return await _locationReadOnlyRepository.GetAllWardsByDistrictCodeAsync(request.DistrictCode, cancellationToken);
    }
}