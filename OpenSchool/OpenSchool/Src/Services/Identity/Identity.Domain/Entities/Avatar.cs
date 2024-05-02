namespace Identity.Domain.Entities;

[Table(TableName.Avatar)]
public class Avatar : PersonalizedEntityAuditBase
{
    public string FileName { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}