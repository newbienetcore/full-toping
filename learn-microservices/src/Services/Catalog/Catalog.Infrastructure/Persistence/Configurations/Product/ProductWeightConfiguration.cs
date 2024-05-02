using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductWeightConfiguration : EntityConfiguration<ProductWeight>
{
    public override void Configure(EntityTypeBuilder<ProductWeight> builder)
    {
        base.Configure(builder);

        #region Columns

        builder
            .HasKey(e => new { e.ProductId, e.WeightCategoryId });
        
        builder
            .Property(e => e.Code)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder
            .Property(e => e.Description)
            .IsRequired(false);

        #endregion
    }
}