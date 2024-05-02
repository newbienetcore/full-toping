using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductVariantAttributeConfiguration : EntityConfiguration<ProductVariantAttribute>
{
    public override void Configure(EntityTypeBuilder<ProductVariantAttribute> builder)
    {
        base.Configure(builder);
        
        #region Columns

        builder
            .HasKey(sc => new { sc.ProductVariantId, sc.AttributeId});

        builder
            .Property(e => e.Value)
            .HasMaxLength(255);

        #endregion
    }
}