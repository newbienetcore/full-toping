namespace Identity.Domain.Entities;

[Table(TableName.Permission)]
public class Permission : EntityAuditBase
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int Exponent { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public ICollection<RolePermission> RolePermissions { get; set; }
    
    #endregion
}