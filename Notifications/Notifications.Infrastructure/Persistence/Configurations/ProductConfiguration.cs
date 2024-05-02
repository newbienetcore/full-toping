using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Name).HasMaxLength(250).IsRequired();

        builder.Property(a => a.Image).HasMaxLength(250).IsRequired();

        builder.Property(a => a.Price).IsRequired();
    }
}