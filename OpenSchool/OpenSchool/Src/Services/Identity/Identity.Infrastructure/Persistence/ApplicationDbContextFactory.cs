using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder
            .UseNpgsql(@"Host=localhost:5433;Database=identity_db;Uid=admin;Pwd=admin", 
                opts =>
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds); 
                }
            );
        
        var context = new ApplicationDbContext(optionsBuilder.Options);
        
        return context;
    }
}