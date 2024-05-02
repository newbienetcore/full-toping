namespace Catalog.API.Constants;

public static class ErrorCode
{
    public const string NameIsRequired = "NAME_IS_REQUIRED";
    public const string NameInValid = "NAME_INVALID";
    public const string NameMinLength = "NAME_MIN_LENGTH_8";
    public const string NameMaxLength = "NAME_MAX_LENGTH_250";
    
    public const string NotFound = "NOT_FOUND";
    
    public const string ProductNotFound = "PRODUCT_NOT_FOUND";
    public const string ProductDuplicate = "PRODUCT_DUPLICATE";
    public const string ProductNotExist = "PRODUCT_NOT_EXIST";
    public const string InvalidProductId = "INVALID_PRODUCT_ID";
    
    public const string CategoryNotFound = "CATEGORY_NOT_FOUND";
    public const string CategoryDuplicate = "CATEGORY_DUPLICATE";
    public const string CategoryNotExist = "CATEGORY_NOT_EXIST";
    public const string InvalidCategoryId = "INVALID_CATEGORY_ID";
    
    public const string BrandNotFound = "BRAND_NOT_FOUND";
    public const string BrandDuplicate = "BRAND_DUPLICATE";
    public const string BrandNotExist = "BRAND_NOT_EXIST";
    public const string InvalidBrandId = "INVALID_BRAND_ID";
    
    public const string FileIsEmpty = "FILE_IS_EMPTY";
    public const string FileNotSupported = "FILE_NOT_SUPPORTED";
    public const string FileNotExist = "FILE_NOT_EXIST";
}