namespace Notifications.Application.Common.Exceptions;

public static class ErrorCode
{
    public const string NameMinLength5MaxLength50 = "NAME_MIN_LENGTH_5_MAX_LENGTH_50";
    public const string CodeMinLength5MaxLength50 = "CODE_MIN_LENGTH_5_MAX_LENGTH_50";
    public const string ApplicationNotFound = "APPLICATION_NOT_FOUND";
    public const string NotificationGroupNotFound = "NOTIFICATION_GROUP_NOT_FOUND";
    public const string FieldIsRequired = "FIELD_IS_REQUIRED";
    public const string UserNotFound = "USER_NOT_FOUND";

}