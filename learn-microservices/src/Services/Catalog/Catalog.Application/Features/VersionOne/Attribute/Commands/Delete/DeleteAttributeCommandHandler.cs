using AutoMapper;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class DeleteAttributeCommandHandler : BaseCommandHandler, IRequestHandler<DeleteAttributeCommand, Guid>
{
    private readonly IAttributeWriteOnlyRepository _attributeWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public DeleteAttributeCommandHandler(
        IServiceProvider provider, 
        IAttributeWriteOnlyRepository attributeWriteOnlyRepository, 
        IStringLocalizer<Resources> localizer
        ) : base(provider)
    {
        _attributeWriteOnlyRepository = attributeWriteOnlyRepository;
        _localizer = localizer;
    }
    
    public async Task<Guid> Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _attributeWriteOnlyRepository.GetByIdAsync(request.AttributeId, cancellationToken);
        if (attribute == null)
        {
            throw new BadRequestException(_localizer["attribute_does_not_exist_or_was_deleted"].Value);
        }

        await _attributeWriteOnlyRepository.DeleteAsync(attribute, cancellationToken);
        await _attributeWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        return request.AttributeId;
    }
}