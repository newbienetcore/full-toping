using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id).ValueGeneratedOnAdd(); 
        
        builder.Property(u => u.Code).HasMaxLength(50).IsUnicode(false).IsRequired();
        
        builder.Property(u => u.UserName).HasMaxLength(50).IsUnicode(false).IsRequired();
        
        builder.Property(u => u.FullName).HasMaxLength(250).IsRequired(false);
        
        builder.Property(u => u.Email).HasMaxLength(250).IsUnicode(false).IsRequired(false);
        
        builder.Property(u => u.CountryCode).HasMaxLength(50).IsUnicode(false).IsRequired();

        builder.Property(u => u.Gender).IsRequired(false);
        
        builder.Property(u => u.AccountType).IsRequired(false);

        builder.Property(u => u.AvatarUrl).HasMaxLength(250).IsUnicode(false).IsRequired(false);
        
        builder.Property(u => u.PhoneNumber).HasMaxLength(50).IsUnicode(false).IsRequired(false);
        
        builder.HasOne(u => u.UserFirebaseToken)
            .WithOne(t => t.User)
            .HasForeignKey<UserFirebaseToken>(t => t.UserId); 
        
        builder.HasMany(u => u.Notifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .IsRequired();
        
        builder.HasMany(u => u.NotificationTypeUsers)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .IsRequired();
    }
}