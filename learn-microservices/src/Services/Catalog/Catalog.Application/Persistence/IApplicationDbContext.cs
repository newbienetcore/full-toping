using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Persistence;

namespace Catalog.Application.Persistence;

public interface IApplicationDbContext : IAppDbContext
{
    #region DbSet
    
    #region Asset

    public DbSet<Asset> Assets { get; }

    #endregion

    #region Category

    public DbSet<Category> Categories { get; }

    #endregion

    #region Supplier

    public DbSet<Supplier> Suppliers { get; }

    #endregion
    
    #region Products

    public DbSet<Attribute> Attributes { get; }
    public DbSet<Product> Products { get;  }
    public DbSet<ProductAsset> ProductAssets { get; }
    public DbSet<ProductCategory> ProductCategories { get; }
    public DbSet<ProductPricing> ProductPricings{ get; }
    public DbSet<ProductReview> ProductReviews { get; }
    public DbSet<ProductSupplier> ProductSuppliers { get; }
    public DbSet<ProductVariant> ProductVariants { get; }
    public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; }
    public DbSet<ProductVariantSpecification> ProductVariantSpecifications { get; }
    public DbSet<ProductWeight> ProductWeights { get; }
    
    #endregion

    #region Discount

    public DbSet<ProductDiscount> ProductDiscounts { get; }
    
    public DbSet<CategoryDiscount> CategoryDiscounts { get; }
    
    #endregion

    #region Weight
    
    public DbSet<Weight> Weights { get; }
    
    #endregion

    #region Location

    public DbSet<LocationProvince> Provinces { get; set; }
    public DbSet<LocationDistrict> Districts { get; set; }
    public DbSet<LocationWard> Wards { get; set; }


    #endregion
    
    #endregion
}