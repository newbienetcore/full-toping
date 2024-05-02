namespace Identity.Domain.Entities;

[Table(TableName.MFA)]
public class MFA : EntityAuditBase
{
    public Guid UserId { get; set; }

    public MFAType Type { get; set; } = MFAType.None;

    public bool Enabled { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual User User { get; set; }
    
    #endregion
}