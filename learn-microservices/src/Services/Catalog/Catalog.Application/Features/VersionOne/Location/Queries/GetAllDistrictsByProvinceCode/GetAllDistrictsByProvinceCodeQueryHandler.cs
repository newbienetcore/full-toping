using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;
 
public class GetAllDistrictsByProvinceCodeQueryHandler : BaseQueryHandler, IRequestHandler<GetAllDistrictsByProvinceCodeQuery, IList<LocationDistrictDto>>
{
    private readonly ILocationReadOnlyRepository _locationReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public GetAllDistrictsByProvinceCodeQueryHandler(
        IMapper mapper,
        ILocationReadOnlyRepository locationReadOnlyRepository,
        IStringLocalizer<Resources> localizer
        ) : base(mapper)
    {
        _locationReadOnlyRepository = locationReadOnlyRepository;
        _localizer = localizer;
    }

    public async Task<IList<LocationDistrictDto>> Handle(GetAllDistrictsByProvinceCodeQuery request, CancellationToken cancellationToken)
    {
        var province = await _locationReadOnlyRepository.GetProvinceByCodeAsync(request.ProvinceCode, cancellationToken);
        if (province == null)
        {
            throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"].Value);
        }
        
        return await _locationReadOnlyRepository.GetAllDistrictsByProvinceCodeAsync(request.ProvinceCode, cancellationToken);
    }
}