using Notifications.Application.Common.Mappings;
using Notifications.Domain.Entities;

namespace Notifications.Application.NotificationGroups.Models;

public class NotificationGroupDto : IMapFrom<NotificationGroup>
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string[]? Variables { get; set; }
    public string[]? SupportedUserLevel { get; set; }
    public Guid AppId { get; set; }
    public AppDto App { get; set; }
}