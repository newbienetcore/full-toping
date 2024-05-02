using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class NotificationGroupConfiguration : IEntityTypeConfiguration<NotificationGroup>
{
    public void Configure(EntityTypeBuilder<NotificationGroup> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Code).HasMaxLength(50).IsUnicode(false).IsRequired();

        builder.Property(n => n.Name).HasMaxLength(250).IsRequired();

        builder.Property(n => n.Variables).IsRequired(false);
        
        builder.Property(n => n.SupportedUserLevel).IsRequired(false);
        
        builder.HasOne(c => c.Parent) 
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId) 
            .IsRequired(false);
        
        builder.HasMany(a => a.NotificationTypes)
            .WithOne(n => n.NotificationGroup)
            .HasForeignKey(n => n.GroupId)
            .IsRequired();

    }
}