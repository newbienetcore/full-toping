namespace Identity.Domain.Entities;

[Table(TableName.SecretKey)]
public class SecretKey : PersonalizedEntityAuditBase
{
    public string Key { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual User User { get; set; }
    
    #endregion
}