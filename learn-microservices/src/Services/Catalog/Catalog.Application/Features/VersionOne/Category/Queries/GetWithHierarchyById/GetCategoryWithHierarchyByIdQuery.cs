using Catalog.Application.DTOs;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryWithHierarchyByIdQuery : BaseAllowAnonymousQuery<IList<CategorySummaryDto>>
{
    public Guid CategoryId { get; init; }

    public GetCategoryWithHierarchyByIdQuery(Guid categoryId) => CategoryId = categoryId;
}