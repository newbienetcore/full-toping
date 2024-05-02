using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class UserFirebaseTokenConfiguration : IEntityTypeConfiguration<UserFirebaseToken>
{
    public void Configure(EntityTypeBuilder<UserFirebaseToken> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();
    }
}