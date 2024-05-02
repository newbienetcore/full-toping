using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : EntityAuditConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(a => a.Id);
        
        builder
            .Property(a => a.RefreshTokenValue)
            .HasMaxLength(512);
       
        builder
            .Property(a => a.CurrentAccessToken)
            .HasMaxLength(512);
        
        builder
            .HasOne(u => u.User)
            .WithMany(uc => uc.RefreshTokens)
            .HasForeignKey(u => u.OwnerId);
    }
}