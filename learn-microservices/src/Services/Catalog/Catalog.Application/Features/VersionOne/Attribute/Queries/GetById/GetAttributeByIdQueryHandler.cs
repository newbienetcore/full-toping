using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetAttributeByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetAttributeByIdQuery, AttributeDto>
{
    private readonly IAttributeReadOnlyRepository _attributeReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public GetAttributeByIdQueryHandler(IMapper mapper, 
        IAttributeReadOnlyRepository attributeReadOnlyRepository,
        IStringLocalizer<Resources> localizer
    ) : base(mapper)
    {
        _attributeReadOnlyRepository = attributeReadOnlyRepository;
        _localizer = localizer;
    }

    public async Task<AttributeDto> Handle(GetAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        var attributeDto = await _attributeReadOnlyRepository.GetAttributeByIdAsync(request.AttributeId, cancellationToken);
        if (attributeDto == null)
        {
            throw new BadRequestException(_localizer["attribute_does_not_exist_or_was_deleted"].Value);
        }

        return attributeDto;
    }
}