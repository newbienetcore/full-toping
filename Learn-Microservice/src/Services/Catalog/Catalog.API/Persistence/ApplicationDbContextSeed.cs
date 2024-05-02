using Catalog.API.Entities;
using ILogger = Serilog.ILogger;

namespace Catalog.API.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        var categories = GetCategories();
        if (!context.Categories.Any() && categories is not null && categories.Any())
        {
            // Seed data category
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
            logger.Information("Seed data for Product database associated with context {DbContextName}", nameof(ApplicationDbContext));
        }

        var brands = GetBrands();
        if (!context.Brands.Any() && brands is not null && brands.Any())
        {
            // Seed data brand
            context.Brands.AddRange(brands);
            await context.SaveChangesAsync();
            logger.Information("Seed data for Brand database associated with context {DbContextName}", nameof(ApplicationDbContext));
        }

        var products = GetProducts();
        if (!context.Products.Any() && products is not null && products.Any())
        {
            // seed data product
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            logger.Information("Seed data for Product database associated with context {DbContextName}", nameof(ApplicationDbContext));
        }
    }

    #region [PRIVATE METHODS]
    
    private static IEnumerable<Category> GetCategories()
    {
        return new List<Category>()
        {
            new Category() { Name = "Iphone", Description = "Description category Iphone" },
            new Category() { Name = "Nokia", Description = "Description category Nokia" },
            new Category() { Name = "Xiaomi", Description = "Description category Xiaomi" }
        };
    }

    private static IEnumerable<Brand> GetBrands()
    {
        return new List<Brand>()
        {
            new Brand() { Name = "Apple", Description = "Description brand Iphone" },
            new Brand() { Name = "Nokia", Description = "Description brand Nokia" },
            new Brand() { Name = "Samsung", Description = "Description brand Samsung" }
        };
    }

    private static IEnumerable<Product> GetProducts()
    {
        return default!;
    }
    
    #endregion [PRIVATE METHODS]
    
    
}