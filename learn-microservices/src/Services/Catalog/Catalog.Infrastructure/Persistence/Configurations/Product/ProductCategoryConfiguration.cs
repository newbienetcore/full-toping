using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductCategoryConfiguration : EntityConfiguration<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);
        
        #region Indexes

        

        #endregion

        #region Columns

        builder
            .HasKey(sc => new { sc.ProductId, sc.CategoryId });

        builder
            .HasOne(e => e.Product)
            .WithMany(e => e.ProductCategories)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(e => e.Category)
            .WithMany(e => e.ProductCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}