using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Category })]
[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class DeleteCategoryCommand : BaseDeleteCommand<object>
{
    public string CategoryId { get; init; }

    public DeleteCategoryCommand(string categoryId) => CategoryId = categoryId;
}