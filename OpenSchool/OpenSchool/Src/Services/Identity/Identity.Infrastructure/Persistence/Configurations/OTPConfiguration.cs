using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class OTPConfiguration : EntityAuditConfiguration<OTP>
{
    public override void Configure(EntityTypeBuilder<OTP> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(a => a.Id);
        
        builder
            .Property(a => a.Otp)
            .HasMaxLength(50)
            .IsUnicode(false);
        
    }
}