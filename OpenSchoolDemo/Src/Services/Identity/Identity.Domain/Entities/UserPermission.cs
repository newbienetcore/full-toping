namespace Identity.Domain.Entities;

[Table(TableName.UserConfig)]
public class UserPermission : EntityAuditBase
{
    public Guid UserId { get; set; }
    public Guid ActionId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual User User { get; set; }
    public virtual Permission Permission { get; set; }
    
    #endregion
}