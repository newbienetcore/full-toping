using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class DeleteAttributeCommand : BaseDeleteCommand<Guid>
{
    public Guid AttributeId { get; init; }

    public DeleteAttributeCommand(Guid attributeId) => AttributeId = attributeId;
}