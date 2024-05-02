namespace Identity.Domain.Entities;

[Table(TableName.RefreshToken)]
public class RefreshToken : PersonalizedEntityAuditBase
{
    public string RefreshTokenValue { get; set; }

    public string CurrentAccessToken { get; set; }

    public DateTime ExpiredDate { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}