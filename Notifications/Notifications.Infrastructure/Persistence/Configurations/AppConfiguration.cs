using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class AppConfiguration : IEntityTypeConfiguration<App>
{
    public void Configure(EntityTypeBuilder<App> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Code).HasMaxLength(50).IsUnicode(false).IsRequired();
        
        builder.Property(a => a.Name).HasMaxLength(250).IsRequired();

        builder.Property(a => a.LogoUrl).HasMaxLength(250).IsUnicode(false).IsRequired(false);

        builder.Property(a => a.SortName).HasMaxLength(50).IsUnicode(false).IsRequired(false);
        
        builder.HasMany(a => a.NotificationGroups)
            .WithOne(n => n.App)
            .HasForeignKey(n => n.AppId)
            .IsRequired();
    }
}