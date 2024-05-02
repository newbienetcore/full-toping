using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class SignInHistoryConfiguration : EntityAuditConfiguration<SignInHistory>
{
    public override void Configure(EntityTypeBuilder<SignInHistory> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(s => s.Id);
    }
}