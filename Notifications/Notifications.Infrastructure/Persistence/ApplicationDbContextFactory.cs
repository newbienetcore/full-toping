using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Notifications.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder
            .UseNpgsql(@"Host=localhost:3306;Database=product_db;Uid=root;Pwd=Dochihung", 
                opts =>
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds); 
                }
                );
        
        var operationalStoreOptions = new OperationalStoreOptions(); // Tạo một instance mới của OperationalStoreOptions

        var context = new ApplicationDbContext(optionsBuilder.Options, Options.Create(operationalStoreOptions));
        
        return context;
    }
}