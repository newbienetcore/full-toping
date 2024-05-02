using Catalog.API.Constants;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(TableNames.ProductImage);
        
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.ImageUrl).HasMaxLength(250).IsRequired();

        builder.Property(a => a.Description).HasMaxLength(1000).IsRequired(false);
    }
}