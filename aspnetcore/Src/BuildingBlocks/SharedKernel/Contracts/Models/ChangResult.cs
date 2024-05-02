using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public class ChangResult
{
    public ChangeType ChangeType { get; set; }

    public List<Change> Changes { get; set; }
}