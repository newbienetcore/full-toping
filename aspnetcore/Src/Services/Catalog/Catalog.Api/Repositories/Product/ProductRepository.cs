using Caching;
using Catalog.Api.Entities;
using Catalog.Api.Persistence;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Auth;
using SharedKernel.Infrastructures;

namespace Catalog.Api.Repositories;

public class ProductRepository : EFCoreWriteOnlyRepository<Product, Guid, CatalogDbContext>, IProductRepository
{
    public ProductRepository(CatalogDbContext context, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching) : base(context, currentUser, sequenceCaching)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return  await FindAll().ToListAsync(cancellationToken);
    }

    public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Product> GetProductByNoAsync(string no, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(x => x.No.Equals(no)).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task CreateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await base.InsertAsync(product, cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await base.UpdateAsync(product, cancellationToken);
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if(product != null) await base.DeleteAsync(product, cancellationToken);
    }
}