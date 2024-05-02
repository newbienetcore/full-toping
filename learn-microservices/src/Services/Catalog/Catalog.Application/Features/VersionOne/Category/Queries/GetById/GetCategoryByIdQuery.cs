using Catalog.Application.DTOs;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class GetCategoryByIdQuery : BaseQuery<CategoryDto>
{
    public Guid CategoryId { get; init; }

    public GetCategoryByIdQuery(Guid categoryId) => CategoryId = categoryId;
}