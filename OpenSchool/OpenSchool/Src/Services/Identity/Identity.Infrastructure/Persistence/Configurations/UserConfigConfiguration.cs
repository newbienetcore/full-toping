using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class UserConfigConfiguration : EntityAuditConfiguration<UserConfig>
{
    public override void Configure(EntityTypeBuilder<UserConfig> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
    }
}