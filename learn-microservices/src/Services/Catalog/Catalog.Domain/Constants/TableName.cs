namespace Catalog.Domain.Constants;

public static class TableName
{
    public const string Category = "categories";
    
    public const string Supplier = "suppliers";
    
    public const string Asset = "assets";

    #region Products
    
    public const string Attribute = "attributes";
    public const string Product = "products";
    public const string ProductVariant = "product_variants";
    public const string ProductVariantAttribute = "product_variant_attributes";
    public const string ProductVariantSpecification = "product_variant_specifications";
    public const string ProductReview = "product_reviews";
    public const string ProductAsset = "product_assets";
    public const string ProductPricing = "product_pricing";
    public const string ProductSupplier = "product_suppliers";
    public const string ProductCategory = "product_categories";
    public const string ProductWeight = "product_weights";
    
    #endregion
    
    #region Discounts
    
    public const string ProductDiscount = "product_discounts";
    public const string CategoryDiscount = "category_discounts";
    
    #endregion

    #region Weight

    public const string Weight = "weights";

    #endregion

    #region Location

    public const string Provinces = "provinces";
    
    public const string Districts = "districts";

    public const string Wards = "wards";

    #endregion
}