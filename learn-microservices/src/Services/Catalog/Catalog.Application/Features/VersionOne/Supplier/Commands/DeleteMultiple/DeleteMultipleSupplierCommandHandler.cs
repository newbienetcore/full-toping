using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class DeleteMultipleSupplierCommandHandler : BaseCommandHandler, IRequestHandler<DeleteMultipleSupplierCommand, IList<Guid>>
{
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    public DeleteMultipleSupplierCommandHandler(
        ISupplierReadOnlyRepository supplierReadOnlyRepository,
        ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
        IStringLocalizer<Resources> localizer,
        IServiceProvider provider
        ) : base(provider)
    {
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
        _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
        _localizer = localizer;
    }

    public async Task<IList<Guid>> Handle(DeleteMultipleSupplierCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids is null || !request.Ids.Any())
        {
            throw new BadRequestException(_localizer["common_list_id_must_not_be_empty"]);
        }
        
        var suppliers = await _supplierReadOnlyRepository.GetListSupplierByIdsAsync(request.Ids, cancellationToken);

        var isMissing = suppliers is null || !suppliers.Any() || request.Ids.Except(suppliers.Select(e => e.Id)).Any();
        if (isMissing)
        {
            throw new BadRequestException(_localizer["supplier_ids_is_valid"].Value);
        }
        
        await _supplierWriteOnlyRepository.DeleteMultipleSupplierAsync(suppliers, cancellationToken);
        await _supplierWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        return request.Ids;
    }
}