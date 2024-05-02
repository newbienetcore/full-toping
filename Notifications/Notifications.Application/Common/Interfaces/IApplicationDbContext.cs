using Microsoft.EntityFrameworkCore;
using Notifications.Domain.Entities;

namespace Notifications.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<App> Apps { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserFirebaseToken> UserFirebaseTokens { get; set; }
    public DbSet<ChannelTemplate> ChannelTemplates { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationGroup> NotificationGroups { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }
    public DbSet<NotificationTypeChannel> NotificationTypeChannels { get; set; }
    public DbSet<NotificationTypeChannelConfig> NotificationTypeChannelConfigs { get; set; }
    public DbSet<NotificationTypeChannelTemplate> NotificationTypeChannelTemplates { get; set; }
    public DbSet<NotificationTypeUser> NotificationTypeUsers { get; set; }
    public DbSet<Product> Products { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}