using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(255)
            .IsRequired(false);
        
        builder
            .Property(e => e.LastModifiedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(e => e.LastModifiedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);
        
        builder
            .Property(e => e.DeletedDate)
            .IsRequired(false);
        
        builder
            .Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired(false);
        
    }
}