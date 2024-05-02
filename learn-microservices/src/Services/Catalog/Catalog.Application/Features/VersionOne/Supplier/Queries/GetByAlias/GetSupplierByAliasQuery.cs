using Catalog.Application.DTOs;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Supplier })]
[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class GetSupplierByAliasQuery : BaseQuery<SupplierDto>
{
    public string Alias { get; init; }

    public GetSupplierByAliasQuery(string alias) => Alias = alias;
}