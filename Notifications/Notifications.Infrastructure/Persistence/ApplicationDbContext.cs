using System.Reflection;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Abstractions.Common.Interfaces;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<User>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,  
        IOptions<OperationalStoreOptions> operationalStoreOptions) 
        : base(options, operationalStoreOptions)
    {
        
    }
    
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        this.ApplyAuditFieldsToModifiedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        this.ApplyAuditFieldsToModifiedEntities();
        return base.SaveChanges();
    }
    
    #region [PRIVATE METHOD]
    private void ApplyAuditFieldsToModifiedEntities()
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreationAuditEntity creationAuditEntity)
                    {
                        creationAuditEntity.CreatedTime = DateTime.UtcNow;
                        entry.State = EntityState.Added;
                    }
                    if (entry.Entity is IDeletionAuditEntity deletion)
                    {
                        deletion.Deleted = false;
                    }
                    break;
                case EntityState.Modified:
                    Entry(entry.Entity).Property("Id").IsModified = false;
                    if (entry.Entity is IModificationAuditEntity modificationAuditEntity)
                    {
                        modificationAuditEntity.ModifiedTime = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }
                    break;
                case EntityState.Deleted:
                    Entry(entry.Entity).Property("Id").IsModified = false;
                    if (entry.Entity is IDeletionAuditEntity deletionAuditEntity)
                    {
                        deletionAuditEntity.Deleted = true;
                        deletionAuditEntity.DeletedTime = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
    }
    #endregion [PRIVATE METHOD]
}