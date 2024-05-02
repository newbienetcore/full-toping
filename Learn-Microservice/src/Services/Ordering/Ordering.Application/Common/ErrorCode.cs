namespace Ordering.Application.Common;

public static class ErrorCode
{
    public const string NameMinLength5MaxLength50 = "NAME_MIN_LENGTH_5_MAX_LENGTH_50";
    public const string CodeMinLength5MaxLength50 = "CODE_MIN_LENGTH_5_MAX_LENGTH_50";
    public const string ApplicationNotFound = "APPLICATION_NOT_FOUND";
    public const string FieldIsRequired = "FIELD_IS_REQUIRED";
    public const string OrderNotFound = "ORDER_NOT_FOUND";
    public const string Success = "SUCCESS";
    public const string Error = "ERROR";
}