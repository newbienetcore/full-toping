using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class UpdateAttributeCommandHandler : BaseCommandHandler, IRequestHandler<UpdateAttributeCommand, AttributeDto>
{
    private readonly IAttributeWriteOnlyRepository _attributeWriteOnlyRepository;
    private readonly IAttributeReadOnlyRepository _attributeReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    
    public UpdateAttributeCommandHandler(
        IServiceProvider provider, 
        IAttributeWriteOnlyRepository attributeWriteOnlyRepository, 
        IAttributeReadOnlyRepository attributeReadOnlyRepository, 
        IStringLocalizer<Resources> localizer,
        IMapper mapper
        ) : base(provider)
    {
        _attributeWriteOnlyRepository = attributeWriteOnlyRepository;
        _attributeReadOnlyRepository = attributeReadOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }
  

    public async Task<AttributeDto> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
    {
        var codeDuplicate = await _attributeReadOnlyRepository.IsDuplicate(null, request.Key, request.Value, cancellationToken);
        if (codeDuplicate == string.Empty)
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }
        
        var attribute = await _attributeWriteOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (attribute == null)
        {
            throw new BadRequestException(_localizer["attribute_does_not_exist_or_was_deleted"].Value);
        }

        attribute = _mapper.Map(request, attribute);
        attribute.Key = attribute.Value.ToUpper();

        await _attributeWriteOnlyRepository.UpdateAsync(attribute, cancellationToken);
        await _attributeWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        var attributeDto = _mapper.Map<AttributeDto>(attribute);
        
        return attributeDto;

    }
}