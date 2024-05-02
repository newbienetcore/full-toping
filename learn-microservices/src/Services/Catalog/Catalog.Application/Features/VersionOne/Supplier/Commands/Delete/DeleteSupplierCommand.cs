using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Supplier })]
[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class DeleteSupplierCommand : BaseDeleteCommand<object>
{
    public string SupplierId { get; init; }

    public DeleteSupplierCommand(string supplierId) => SupplierId = supplierId;
}