using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace Identity.Infrastructure.Persistence.Configurations;

public class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>  where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .HasKey(e => e.Id);
    }
}