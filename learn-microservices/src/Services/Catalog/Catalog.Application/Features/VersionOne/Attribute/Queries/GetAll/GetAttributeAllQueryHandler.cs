using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetAttributeAllQueryHandler : BaseQueryHandler, IRequestHandler<GetAttributeAllQuery, IList<AttributeDto>>
{
    private readonly IAttributeReadOnlyRepository _attributeReadOnlyRepository;
    public GetAttributeAllQueryHandler(IMapper mapper, 
        IAttributeReadOnlyRepository attributeReadOnlyRepository
        ) : base(mapper)
    {
        _attributeReadOnlyRepository = attributeReadOnlyRepository;
    }

    public async Task<IList<AttributeDto>> Handle(GetAttributeAllQuery request, CancellationToken cancellationToken)
    {
        return await _attributeReadOnlyRepository.GetAllAsync(cancellationToken);
    }
}