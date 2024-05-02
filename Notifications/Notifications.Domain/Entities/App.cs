using System.Collections;

namespace Notifications.Domain.Entities;

public class App
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string SortName { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public ICollection<NotificationGroup> NotificationGroups { get; set; }
    #endregion [REFERENCE PROPERTIES]
    
}