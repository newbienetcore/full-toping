using Catalog.Application.Mappings;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class CreateAttributeCommand : BaseCommand<Guid>, IMapFrom<Attribute>
{
    public string Key { get; init; }
    public string Value { get; init; }
}