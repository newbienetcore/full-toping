using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class NotificationTypeChannelTemplateConfiguration : IEntityTypeConfiguration<NotificationTypeChannelTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTypeChannelTemplate> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();
    }
}
