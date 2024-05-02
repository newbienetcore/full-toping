using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    #region [DB SET]
    
    public DbSet<Entities.Customer> Customers { get; set; }
    
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.UserName).IsUnique();
        
        modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.EmailAddress).IsUnique();

        
    }
}