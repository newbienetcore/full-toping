using Catalog.Application.DTOs;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

public class GetCategoryByAliasQuery : BaseAllowAnonymousQuery<CategoryDto>
{
    public string Alias { get; init; }

    public GetCategoryByAliasQuery(string alias) => Alias = alias;
}