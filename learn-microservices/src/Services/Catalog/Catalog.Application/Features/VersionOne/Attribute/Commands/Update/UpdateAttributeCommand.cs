using Catalog.Application.DTOs;
using Catalog.Application.Mappings;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class UpdateAttributeCommand : BaseUpdateCommand<AttributeDto>, IMapFrom<Attribute>
{
    public Guid Id { get; init; }
    public string Key { get; init; }
    public string Value { get; init; }
}