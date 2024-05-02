using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class UserPermissionConfiguration : EntityAuditConfiguration<UserPermission>
{
    public override void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(ur => new { ur.UserId, ur.PermissionId });

        builder
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);
        
        builder
            .HasOne(up => up.Permission)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.PermissionId);
    }
}