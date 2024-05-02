using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetSupplierPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetSupplierPagingQuery, IPagedList<SupplierDto>>
{
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    
    public GetSupplierPagingQueryHandler(
        IMapper mapper,
        ISupplierReadOnlyRepository supplierReadOnlyRepository
        ) : base(mapper)
    {
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
    }

    public async Task<IPagedList<SupplierDto>> Handle(GetSupplierPagingQuery request, CancellationToken cancellationToken)
    {
        var result = await _supplierReadOnlyRepository.GetPagingResultAsync(request.PagingRequest, cancellationToken);
        return result;
    }
}