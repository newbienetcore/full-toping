using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder
            .UseMySql(
                connectionString: @"Server=localhost; Port=3306; Database=catalog_db; Uid=root; Pwd=root; SslMode=Preferred;", 
                serverVersion: new MySqlServerVersion(new Version(8, 2, 0)),
                mySqlOptionsAction: mySqlOptions =>
                {
                    mySqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                    mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }
            );
        
        var context = new ApplicationDbContext(optionsBuilder.Options);
        
        return context;
    }
}