using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextSeed(ILogger logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if(!_context.Database.IsSqlServer()) return;
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(new Order()
                {
                    UserName = "customer1", FirstName = "customer", LastName = "1",
                    EmailAddress = "customer1@local.com",
                    ShippingAddress = "Wollongong", InvoiceAddress = "Australia", TotalPrice = 250
                });
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}