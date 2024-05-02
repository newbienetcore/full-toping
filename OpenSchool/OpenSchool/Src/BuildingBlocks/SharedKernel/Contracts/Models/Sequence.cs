using SharedKernel.Domain;

namespace SharedKernel.Contracts;

public class Sequence : EntityAuditBase
{
    public string Table { get; set; }

    public long SeqNo { get; set; }
}