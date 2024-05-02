using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : EntityConfiguration<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);

        #region Indexes

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
        
        builder
            .HasIndex(e => e.Alias)
            .IsUnique();
        
        builder.HasIndex(e => e.Barcode)
            .IsUnique();
        
        builder.HasIndex(e => e.Serial)
            .IsUnique();

        #endregion

        #region Columns
        
        builder
            .Property(e => e.Name)
            .HasMaxLength(255);

        builder
            .Property(e => e.Alias)
            .HasMaxLength(255);
        
        builder
            .Property(e => e.BriefDescription)
            .IsRequired(false);
        
        builder
            .Property(e => e.TotalDescription)
            .IsRequired(false);
        
        builder
            .Property(e => e.Barcode)
            .HasMaxLength(255);    
        
        builder
            .Property(e => e.Serial)
            .HasMaxLength(255);

        builder
            .Property(e => e.Sets)
            .IsRequired(false);
        
        builder
            .Property(e => e.Status)
            .HasDefaultValue(true);
        
        builder
            .Property(e => e.RelatedType)
            .IsRequired(false);
        
        builder
            .Property(e => e.WarrantyInfo)
            .IsRequired(false);
        
        builder
            .HasMany(sc => sc.ProductVariantAttributes)
            .WithOne(s => s.ProductVariant)
            .HasForeignKey(sc => sc.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        builder.HasMany(p => p.ProductPricings)
            .WithOne(pd => pd.ProductVariant)
            .HasForeignKey(pd => pd.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductVariantSpecifications)
            .WithOne(pd => pd.ProductVariant)
            .HasForeignKey(pd => pd.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(e => e.ProductAsset)
            .WithOne(e => e.ProductVariant)
            .HasForeignKey<ProductAsset>(e => e.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        #endregion

    }
}