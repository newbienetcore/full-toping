using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        #region Indexes

        

        #endregion

        #region Columns

            builder.Property(p => p.ViewCount)
                .HasDefaultValue(0);
            
            builder.Property(p => p.Status)
                .HasDefaultValue(true);

            builder.Property(p => p.HomeFlag)
                .HasDefaultValue(false);

            builder.Property(p => p.HotFlag)
                .HasDefaultValue(false);

            builder.Property(p => p.IsBestSelling)
                .HasDefaultValue(false);

            builder.Property(p => p.IsNew)
                .HasDefaultValue(false);

            builder.Property(p => p.IsHot)
                .HasDefaultValue(false);

            // Relationships
            builder
                .HasOne(e => e.ProductWeight)
                .WithOne(e => e.Product)
                .HasForeignKey<ProductWeight>(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Navigations
            builder.HasMany(p => p.ProductDiscounts)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            
            builder.HasMany(p => p.ProductVariants)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(e => e.ProductAssets)
                .WithOne(e => e.Product)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(p => p.ProductReviews)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        
            builder
                .HasMany(e => e.ProductSuppliers)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            #endregion

    }
}