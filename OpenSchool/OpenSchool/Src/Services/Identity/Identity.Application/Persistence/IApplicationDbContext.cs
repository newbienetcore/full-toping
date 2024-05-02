using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.EFCore;

namespace Identity.Application.Persistence;

public interface IApplicationDbContext : ICoreDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Avatar> Avatars { get; set; }
    DbSet<UserConfig> UserConfigs { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<UserPermission> UserPermissions { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<SignInHistory> SignInHistories { get; set; }
    DbSet<SecretKey> SecretKeys { get; set; }
    DbSet<OTP> OTPs { get; set; }
    DbSet<MFA> MFAs { get; set; }
    
}