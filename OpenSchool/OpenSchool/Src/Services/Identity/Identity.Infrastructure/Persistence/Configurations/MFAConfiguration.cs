using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class MFAConfiguration : EntityAuditConfiguration<MFA>
{
    public override void Configure(EntityTypeBuilder<MFA> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(a => a.Id);
        
        builder.Property(cd => cd.Type)
            .HasConversion(
                v => v.ToString(), 
                v => (MFAType)Enum.Parse(typeof(MFAType), v) 
            );
    }
}