using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : EntityAuditConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
        
        builder
            .HasIndex(r => r.Code)
            .IsClustered();
            
        builder
            .Property(r => r.Code)
            .HasMaxLength(50);

        builder
            .Property(r => r.Name)
            .HasMaxLength(100);
    }
}