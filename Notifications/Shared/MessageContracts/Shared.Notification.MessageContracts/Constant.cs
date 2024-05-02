namespace Shared.Notification.MessageContracts;

public static class Constant
{
    public const string RegexPhoneNumber = "^\\+?[0-9][0-9]{3,24}$";
    public const string RegexEmailRfc5322 =
        "(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
    
    public static class TopicConstant
    {
        public const string PushNotificationGroup = "ichiba_notification_push_notification_group_topic";
    }

    public static class ErrorCode
    {
        public const string UserIdsOrDestinationsIsRequired = "USERIDS_OR_DESTINATIONS_IS_REQUIRED";
        public const string DestinationsMustBeEmailOrPhoneNumber = "DESTINATIONS_MUS_BE_EMAIL_OR_PHONENUMBER";
    }
}