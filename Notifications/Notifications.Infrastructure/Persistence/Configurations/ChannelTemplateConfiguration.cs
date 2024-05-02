using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class ChannelTemplateConfiguration : IEntityTypeConfiguration<ChannelTemplate>
{
    public void Configure(EntityTypeBuilder<ChannelTemplate> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Code).HasMaxLength(50).IsUnicode(false).IsRequired();

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired();

        builder.Property(c => c.Channel).IsRequired();

        builder.Property(c => c.Subject).HasMaxLength(250).IsRequired();
        
        builder.Property(c => c.Content).HasMaxLength(int.MaxValue);

        builder.Property(c => c.FileUrl).HasMaxLength(250);
    }
}