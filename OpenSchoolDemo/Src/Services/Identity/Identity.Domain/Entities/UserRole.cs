namespace Identity.Domain.Entities;

[Table(TableName.UserRole)]
public class UserRole : EntityAuditBase
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    #region [REFRENCE PROPERTIES]
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    
    #endregion
}