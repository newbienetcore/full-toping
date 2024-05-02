namespace Notifications.Domain.Entities;

public class UserFirebaseToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string DeviceToken { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual User User { get; set; }
    #endregion [REFERENCE PROPERTIES]
}