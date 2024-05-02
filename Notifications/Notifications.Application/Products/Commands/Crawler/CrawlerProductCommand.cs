using System.Globalization;
using MediatR;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Products.Models;
using PuppeteerSharp;
using Product = Notifications.Domain.Entities.Product;

namespace Notifications.Application.Products.Commands.Crawler;

public class CrawlerProductCommandResponse
{
    public List<ProductDto> Products { get; set; }
}

public class CrawlerProductCommand : IRequest<CrawlerProductCommandResponse>
{
    public string Source { get; set; }
}


public class CrawlerProductCommandHandler : IRequestHandler<CrawlerProductCommand, CrawlerProductCommandResponse>
{
    
    private readonly IApplicationDbContext _context;

    public CrawlerProductCommandHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    
    public async Task<CrawlerProductCommandResponse> Handle(CrawlerProductCommand request, CancellationToken cancellationToken)
    {

        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GoToAsync($"{request.Source}");
        
        var productElements = await page.QuerySelectorAllAsync(".cdt-product");

        if (productElements is null || productElements.Length < 1) return default!;

        var products = new List<Product>();
        var i = 1;
        foreach (var productElement in productElements)
        {
            Console.WriteLine($"===================== {i++}");
            var product = new Product();

            var imageElement = await productElement.QuerySelectorAsync("img");
            product.Image = "";

            var nameElement = await productElement.QuerySelectorAsync(".cdt-product__name");
            product.Name = await nameElement.EvaluateFunctionAsync<string>("node => node.innerText");

            var priceElement = await productElement.QuerySelectorAsync(".strike-price");
            if(priceElement is null)
                priceElement = await productElement.QuerySelectorAsync(".price");
            
            var priceString = await priceElement.EvaluateFunctionAsync<string>("node => node.innerText");
            product.Price = ConvertCurrency(priceString);
            
            products.Add(product);
        }
        
        var p = products;
        await _context.Products.AddRangeAsync(products, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new CrawlerProductCommandResponse()
        {
            Products = products.Select(p => new ProductDto(){Id = p.Id, Name = p.Name, Image = p.Image, Price = p.Price}).ToList()
        };
    }
    
    // Regex
    private static decimal ConvertCurrencyRegex(string currencyValue)
    {
        return default!;
    }
    
    private static decimal ConvertCurrency(string currencyValue)
    {
        int endIndex = currencyValue.IndexOf("₫");

        string sanitizedValue = currencyValue.Substring(0, endIndex);
        
        sanitizedValue = sanitizedValue.Replace(".", "").Replace(",", "");
        
        decimal convertedValue = decimal.Parse(sanitizedValue, CultureInfo.InvariantCulture);

        return convertedValue;
    }
    
}