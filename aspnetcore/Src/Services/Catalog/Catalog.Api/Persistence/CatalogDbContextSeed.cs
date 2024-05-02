using Catalog.Api.Entities;
using ILogger = Serilog.ILogger;

namespace Catalog.Api.Persistence;

public class CatalogDbContextSeed
{
    public static async Task SeedProductAsync(CatalogDbContext context, ILogger logger)
    {
        if (!context.Products.Any())
        {
            await context.AddRangeAsync(GetProducts());
            
            await context.SaveChangesAsync();
            logger.Information("Seeded data for Product DB associated with context CatalogDbContext");
        }
    }

    private static IEnumerable<Product> GetProducts()
    {
        return new List<Product>()
        {
            new()
            {
                No = "Lotus",
                Name = "Esprit",
                Summary = "Nondisplaced fracture of greater trochanter of right femur",
                Description = "Nondisplaced fracture of greater trochanter of right femur",
                Price = (decimal)177940.49
            },
            new()
            {
                No = "Cadillac",
                Name = "CTS",
                Summary = "Carbuncle of trunk",
                Description = "Carbuncle of trunk",
                Price = (decimal)114728.21
            }
        };
    }
}