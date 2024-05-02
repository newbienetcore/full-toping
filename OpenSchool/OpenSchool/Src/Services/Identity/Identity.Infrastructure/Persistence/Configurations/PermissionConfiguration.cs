using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : EntityAuditConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(a => a.Id);
        
        builder
            .Property(a => a.Code)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder
            .Property(a => a.Name)
            .HasMaxLength(100);

        builder
            .Property(a => a.Description)
            .HasMaxLength(255);
    }
}