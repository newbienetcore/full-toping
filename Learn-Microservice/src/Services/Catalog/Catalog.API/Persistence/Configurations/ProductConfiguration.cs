using Catalog.API.Constants;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Product);

        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.HasIndex(a => a.No).IsUnique();
        
        // required, varchar(50)
        builder.Property(p => p.No).HasMaxLength(50).IsUnicode(false).IsRequired();
        
        // required, nvarchar(250)
        builder.Property(p => p.Name).HasMaxLength(250).IsRequired();

        // required, nvarchar(max)
        builder.Property(p => p.Summary).IsRequired().HasColumnType("nvarchar(max)");
        
        // not required, text
        builder.Property(p => p.Description).IsRequired().HasColumnType("text");
        
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
        
        builder.Property(p => p.StockQuantity).IsRequired().HasDefaultValue(0);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired();

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .HasPrincipalKey(b => b.Id)
            .IsRequired();

        builder.HasMany(p => p.ProductImages)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .IsRequired();
    }
}