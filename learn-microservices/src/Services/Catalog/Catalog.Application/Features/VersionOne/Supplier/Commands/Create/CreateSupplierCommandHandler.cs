using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class CreateSupplierCommandHandler : BaseCommandHandler, IRequestHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    
    public CreateSupplierCommandHandler(
        IServiceProvider provider,
        ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
        ISupplierReadOnlyRepository supplierReadOnlyRepository,
        IStringLocalizer<Resources> localizer,
        IMapper mapper
        ) : base(provider)
    {
        _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        request.Alias = request.Name.ToUnsignString();
        
        var codeDuplicate = await _supplierReadOnlyRepository.IsDuplicate(null, request.Email, request.Phone, request.Name, cancellationToken);
        if (!string.IsNullOrWhiteSpace(codeDuplicate))
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }
        
        var supplier = _mapper.Map<Supplier>(request);
        
        await _supplierWriteOnlyRepository.InsertAsync(supplier, cancellationToken);
        await _supplierWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        var supplierDto = _mapper.Map<SupplierDto>(supplier);

        return supplierDto;
    }
}