using Catalog.Api.Entities;
using Catalog.Api.Persistence;
using SharedKernel.Contracts.Repositories;

namespace Catalog.Api.Repositories.Interfaces;

public interface IProductRepository : IEFCoreWriteOnlyRepository<Product, Guid, CatalogDbContext>
{
    Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product> GetProductByNoAsync(string no, CancellationToken cancellationToken = default);
    
    Task CreateProductAsync(Product product, CancellationToken cancellation = default);
    Task UpdateProductAsync(Product product, CancellationToken cancellation = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellation = default);
}