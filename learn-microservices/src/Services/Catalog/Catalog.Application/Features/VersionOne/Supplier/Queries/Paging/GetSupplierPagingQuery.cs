using Catalog.Application.DTOs;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Supplier })]
[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class GetSupplierPagingQuery : BaseQuery<IPagedList<SupplierDto>>
{
    public PagingRequest PagingRequest { get; }

    public GetSupplierPagingQuery(PagingRequest pagingRequest)
    {
        PagingRequest = pagingRequest;
    }
}