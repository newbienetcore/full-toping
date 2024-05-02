using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class AvatarConfiguration : EntityAuditConfiguration<Avatar>
{
    public override void Configure(EntityTypeBuilder<Avatar> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
        
        builder
            .Property(a => a.FileName)
            .HasMaxLength(255);
    }
}