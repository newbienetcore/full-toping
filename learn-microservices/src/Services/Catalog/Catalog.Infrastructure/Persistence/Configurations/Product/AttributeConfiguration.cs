using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class AttributeConfiguration : BaseEntityConfiguration<Attribute>
{
    public override void Configure(EntityTypeBuilder<Attribute> builder)
    {
        base.Configure(builder);

        #region Indexes

        builder
            .HasIndex(e => e.Key)
            .IsUnique();

        #endregion

        #region Columns

        builder
            .Property(e => e.Key)
            .HasMaxLength(255)
            .IsUnicode(false);
        
        builder
            .Property(e => e.Value)
            .HasMaxLength(255);
        
        builder
            .HasMany(sc => sc.ProductVariantAttributes)
            .WithOne(s => s.Attribute)
            .HasForeignKey(sc => sc.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
        
    }
}