using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

public class ApplicationDbContext : AppDbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    #region DbSet
    
    #region Asset

    public DbSet<Asset> Assets { get; set; }

    #endregion

    #region Category

    public DbSet<Category> Categories { get; set; }

    #endregion

    #region Supplier

    public DbSet<Supplier> Suppliers { get; set; }

    #endregion
    
    #region Product

    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Product> Products { get;  }
    public DbSet<ProductAsset> ProductAssets { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductPricing> ProductPricings{ get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductSupplier> ProductSuppliers { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
    public DbSet<ProductVariantSpecification> ProductVariantSpecifications { get; set; }
    public DbSet<ProductWeight> ProductWeights { get; set; }
    
    #endregion

    #region Discount

    public DbSet<ProductDiscount> ProductDiscounts { get; set; }
    
    public DbSet<CategoryDiscount> CategoryDiscounts { get; set; }
    
    #endregion

    #region Weight
    
    public DbSet<Weight> Weights { get; set; }
    
    #endregion

    #region Location

    public DbSet<LocationProvince> Provinces { get; set; }
    public DbSet<LocationDistrict> Districts { get; set; }
    public DbSet<LocationWard> Wards { get; set; }
    
    #endregion
    
    #endregion

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    { 
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified ||
                        e.State == EntityState.Added ||
                        e.State == EntityState.Deleted);

        foreach (var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                {
                    break;
                }
                case EntityState.Modified:
                {
                    Entry(item.Entity).Property("Id").IsModified = false;
                    break;
                }
                case EntityState.Deleted:
                {
                    if (item.Entity is IAuditable)
                    {
                        item.State = EntityState.Modified;
                    }
                    break; 
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var entityTypes = builder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                builder.SetSoftDeleteFilter(entityType.ClrType);
        }
        
        base.OnModelCreating(builder);
    }
}