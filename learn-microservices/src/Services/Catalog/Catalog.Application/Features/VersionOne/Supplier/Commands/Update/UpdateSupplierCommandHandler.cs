using AutoMapper;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class UpdateSupplierCommandHandler : BaseCommandHandler, IRequestHandler<UpdateSupplierCommand, Unit>
{
    private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    public UpdateSupplierCommandHandler(
        ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
        ISupplierReadOnlyRepository supplierReadOnlyRepository,
        IStringLocalizer<Resources> localizer,
        IMapper mapper,
        IServiceProvider provider
        ) : base(provider)
    {
        _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var codeDuplicate = await _supplierReadOnlyRepository.IsDuplicate(null, request.Email, request.Phone, request.Name, cancellationToken);
        if (!string.IsNullOrWhiteSpace(codeDuplicate))
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }

        var supplier = await _supplierWriteOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (supplier is null)
        {
            throw new BadRequestException(_localizer["supplier_does_not_exist_or_was_deleted"].Value);
        }
        
        supplier = _mapper.Map(request, supplier);
        supplier.Alias = supplier.Name.ToUnsignString();
        
        await _supplierWriteOnlyRepository.UpdateSupplierAsync(supplier, cancellationToken);
        await _supplierWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);
        
        return Unit.Value;
    }
}