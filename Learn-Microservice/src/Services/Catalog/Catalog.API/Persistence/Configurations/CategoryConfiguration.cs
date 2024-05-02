using Catalog.API.Constants;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableNames.Category);

        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Name).HasMaxLength(250).IsRequired();
    }
}