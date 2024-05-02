using Catalog.Application.DTOs;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class GetCategoryPagingQuery : BaseQuery<IPagedList<CategoryDto>>
{
    public PagingRequest PagingRequest { get; }

    public GetCategoryPagingQuery(PagingRequest pagingRequest)
    {
        PagingRequest = pagingRequest;
    }
}