using System.Reflection;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Deleted 
                        || e.State == EntityState.Modified);

        foreach (var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                {
                    if (item.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.CreateDate = DateTime.Now;
                        item.State = EntityState.Added;
                    }
                    break;
                }
                case EntityState.Modified:
                {
                    Entry(item.Entity).Property("Id").IsModified = false;
                    if (item.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.LastModifiedDate = DateTime.Now;
                        item.State = EntityState.Modified;
                    }
                    break;
                }
                case EntityState.Deleted:
                {
                    Entry(item.Entity).Property("Id").IsModified = false;
                    break;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}