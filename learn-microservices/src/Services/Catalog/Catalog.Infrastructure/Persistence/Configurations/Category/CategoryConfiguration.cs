using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        #region Indexes
        
        builder
            .HasIndex(e => e.Name)
            .IsUnique();
        
        builder
            .HasIndex(e => e.Alias)
            .IsUnique();

        #endregion
        
        #region Columns
        
        builder
            .Property(e => e.Name)
            .HasMaxLength(255);
        
        builder
            .Property(e => e.Alias)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder
            .Property(e => e.Description)
            .IsRequired(false);

        builder
            .Property(e => e.Status)
            .HasDefaultValue(true);
        
        builder
            .Property(e => e.ParentId)
            .IsRequired(false);

        builder
            .HasOne(c => c.Parent)
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false);
        
        builder
            .HasMany(e => e.CategoryDiscounts)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        #endregion

    }
}