namespace Identity.Domain.Entities;

[Table(TableName.RolePermission)]
public class RolePermission : EntityAuditBase
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual Role Role { get; set; }
    public virtual Permission Permission { get; set; }
    
    #endregion
}