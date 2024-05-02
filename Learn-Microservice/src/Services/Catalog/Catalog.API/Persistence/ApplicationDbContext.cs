using Catalog.API.Entities;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Persistence;

public class ApplicationDbContext : DbContext
{
    #region [DB SET]

    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> Images { get; set; }
    
    #endregion [DB SET]
    
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public ApplicationDbContext() { }
    
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