using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : EntityAuditConfiguration<RolePermission>
{
    public override void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(ur => new { ur.PermissionId, ur.RoleId });

        builder.HasOne(ur => ur.Role)
            .WithMany(u => u.RolePermissions)
            .HasForeignKey(ur => ur.RoleId);

        builder.HasOne(ur => ur.Permission)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(ur => ur.PermissionId);
    }
}