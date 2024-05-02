using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace Identity.Infrastructure.Persistence.Configurations;

public class EntityAuditConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityAuditBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(255);
        
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
            .HasDefaultValue(false);
    }
}