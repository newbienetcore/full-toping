using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enum = System.Enum;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : BaseEntityConfiguration<Asset>
{
    public override void Configure(EntityTypeBuilder<Asset> builder)
    {
        base.Configure(builder);

        #region Indexes

        builder
            .HasIndex(e => e.FileName)
            .IsUnique();
        
        builder
            .HasIndex(e => e.OriginalFileName)
            .IsUnique();

        #endregion

        #region Columns

        builder
            .Property(e => e.FileName)
            .HasMaxLength(256);
        
        builder
            .Property(e => e.OriginalFileName)
            .HasMaxLength(512);

        builder
            .Property(e => e.Description)
            .IsRequired(false);
        
        builder.Property(cd => cd.Type)
            .HasConversion(
                v => v.ToString(),   
                v => (SharedKernel.Application.Enum.FileType)Enum.Parse(typeof(SharedKernel.Application.Enum.FileType), v)  
            );

        builder
            .HasMany(sc => sc.ProductAssets)
            .WithOne(s => s.Asset)
            .HasForeignKey(sc => sc.AssetId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        #endregion
    }
    
}