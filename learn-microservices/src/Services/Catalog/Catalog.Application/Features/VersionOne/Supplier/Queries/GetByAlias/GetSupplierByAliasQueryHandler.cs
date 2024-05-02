using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class GetSupplierByAliasQueryHandler : BaseQueryHandler, IRequestHandler<GetSupplierByAliasQuery, SupplierDto>
{
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    
    public GetSupplierByAliasQueryHandler(
        IMapper mapper,
        ISupplierReadOnlyRepository supplierReadOnlyRepository,
        IStringLocalizer<Resources> localizer
    ) : base(mapper)
    {
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
        _localizer = localizer;
    }

    public async Task<SupplierDto> Handle(GetSupplierByAliasQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierReadOnlyRepository.GetSupplierByAliasWithCachingAsync(request.Alias, cancellationToken);
        if (supplier is null)
        {
            throw new BadRequestException(_localizer["supplier_does_not_exist_or_was_deleted"].Value);
        }

        var supplierDto = _mapper.Map<SupplierDto>(supplier);

        return supplierDto;
    }
}