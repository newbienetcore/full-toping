namespace Shared.Notification.MessageContracts;

public record PushNotificationGroupMessage(
    string Code,
    string Name,
    Guid? ParentId,
    string[]? Variables,
    string[]? SupportedUserLevel,
    Guid AppId);