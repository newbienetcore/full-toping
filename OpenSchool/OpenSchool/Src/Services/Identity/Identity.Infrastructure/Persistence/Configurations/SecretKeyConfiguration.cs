using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class SecretKeyConfiguration : EntityAuditConfiguration<SecretKey>
{
    public override void Configure(EntityTypeBuilder<SecretKey> builder)
    {
        base.Configure(builder);
    }
}