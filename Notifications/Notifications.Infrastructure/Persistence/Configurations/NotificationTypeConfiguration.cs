using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
{
    public void Configure(EntityTypeBuilder<NotificationType> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Code).HasMaxLength(50).IsRequired();

        builder.Property(n => n.Status).IsRequired();
        
        builder.HasMany(a => a.NotificationTypeChannels)
            .WithOne(n => n.NotificationType)
            .HasForeignKey(n => n.NotificationTypeId)
            .IsRequired();
    }
}
