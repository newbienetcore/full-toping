using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired(false);
        
    }
}