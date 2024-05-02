using Catalog.Domain.Entities;
using Catalog.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enum = System.Enum;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductAssetConfiguration : EntityConfiguration<ProductAsset>
{
    public override void Configure(EntityTypeBuilder<ProductAsset> builder)
    {
        base.Configure(builder);

        #region Indexes

        builder
            .HasIndex(sc => new { sc.ProductId, sc.AssetId })
            .IsUnique();

        #endregion

        #region Columns

        builder.HasKey(s => s.Id);
        
        builder.Property(cd => cd.ImageType)
            .HasConversion(
                v => v.ToString(),   
                v => (ProductImageType)Enum.Parse(typeof(ProductImageType), v)  
            );
        
        #endregion
    }
}