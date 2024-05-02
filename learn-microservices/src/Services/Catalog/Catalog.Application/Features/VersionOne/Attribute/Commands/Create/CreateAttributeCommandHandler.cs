using AutoMapper;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class CreateAttributeCommandHandler : BaseCommandHandler, IRequestHandler<CreateAttributeCommand, Guid>
{
    private readonly IAttributeWriteOnlyRepository _attributeWriteOnlyRepository;
    private readonly IAttributeReadOnlyRepository _attributeReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    public CreateAttributeCommandHandler(
        IServiceProvider provider
        ) : base(provider)
    {
        
    }

    public async Task<Guid> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {
        var codeDuplicate = await _attributeReadOnlyRepository.IsDuplicate(null, request.Key, request.Value, cancellationToken);

        if (codeDuplicate == string.Empty)
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }

        var attribute = _mapper.Map<Attribute>(request);
        await _attributeWriteOnlyRepository.InsertAsync(attribute, cancellationToken);
        await _attributeWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        return attribute.Id;
    }
}