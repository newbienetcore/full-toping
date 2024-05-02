namespace Customer.API.Constants;

public static class ErrorCode
{
    public const string CustomerNotFound = "CUSTOMER_NOT_FOUND";
    public const string CustomerUserNameIsExist = "CUSTOMER_USERNAME_IS_EXIST";
    public const string CustomerEmailAddressIsExist = "CUSTOMER_EMAIL_ADDRESS_IS_EXIST";
    public const string CustomerUserNameOrEmailAddressIsExist = "CUSTOMER_USERNAME_OR_EMAIL_ADDRESS_IS_EXIST";
    public const string CustomerDuplicate = "CUSTOMER_DUPLICATE";
    public const string CustomerNotExist = "CUSTOMER_NOT_EXIST";
    public const string InvalidCustomerId = "CUSTOMER_CATEGORY_ID";
}