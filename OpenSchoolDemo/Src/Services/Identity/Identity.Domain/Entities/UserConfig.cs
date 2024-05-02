namespace Identity.Domain.Entities;

[Table(TableName.UserConfig)]
public class UserConfig : PersonalizedEntityAuditBase
{
    public string Json { get; set; }
    
    #region [REFRENCE PROPERTIES]

    public virtual User User { get; set; }

    #endregion
}