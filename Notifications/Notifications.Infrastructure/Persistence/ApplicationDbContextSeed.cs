using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    private readonly ILogger<ApplicationDbContextSeed> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextSeed(ILogger<ApplicationDbContextSeed> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
                await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred while seeding the database;");
            throw;
        }
    }


    public async Task TrySeedAsync()
    {
        if (!_context.Apps.Any())
        {
            await _context.Apps.AddRangeAsync(
                new List<App>()
                {
                    new App() {Code = "1", Name = "App"}
                }
            );
        }

        if (!_context.Users.Any())
        {
            await _context.Users.AddRangeAsync(
                new List<User>()
                {
                    new User() { Code = "1", UserName = "dochihung1", FullName = "Đỗ Chí Hùng 1", CountryCode = "100" },
                    new User() { Code = "2", UserName = "dochihung2", FullName = "Đỗ Chí Hùng 2", CountryCode = "100" },
                    new User() { Code = "3", UserName = "dochihung3", FullName = "Đỗ Chí Hùng 3", CountryCode = "100" },
                    new User() { Code = "4", UserName = "dochihung4", FullName = "Đỗ Chí Hùng 4", CountryCode = "100" },
                    new User() { Code = "5", UserName = "dochihung5", FullName = "Đỗ Chí Hùng 5", CountryCode = "100" }
                    
                }
            );
        }
    }

    public async Task<IEnumerable<App>> GetAllAsync()
    {
        return await _context.Apps.ToListAsync();
    }
}