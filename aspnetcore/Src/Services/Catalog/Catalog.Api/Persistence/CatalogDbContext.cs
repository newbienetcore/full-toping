using Catalog.Api.Entities;
using Catalog.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.EFCore;

namespace Catalog.Api.Persistence;

public class CatalogDbContext : CoreDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
        }
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasIndex(x => x.No)
            .IsUnique();
    }
}