using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class NotificationTypeChannelConfigConfiguration : IEntityTypeConfiguration<NotificationTypeChannelConfig>
{
    public void Configure(EntityTypeBuilder<NotificationTypeChannelConfig> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Enabled).IsRequired();
    }
}
