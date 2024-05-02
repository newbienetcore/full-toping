using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class NotificationTypeChannelConfiguration : IEntityTypeConfiguration<NotificationTypeChannel>
{
    public void Configure(EntityTypeBuilder<NotificationTypeChannel> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Channel).IsRequired();
        
        builder.Property(n => n.Enabled).IsRequired();
        
        builder.HasMany(a => a.NotificationTypeChannelTemplates)
            .WithOne(n => n.NotificationTypeChannel)
            .HasForeignKey(n => n.NotificationTypeChannelId)
            .IsRequired();
        
        builder.HasMany(a => a.NotificationTypeChannelConfigs)
            .WithOne(n => n.NotificationTypeChannel)
            .HasForeignKey(n => n.NotificationTypeChannelId)
            .IsRequired();
    }
}