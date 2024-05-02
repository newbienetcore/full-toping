using Caching;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories;

public class AssetWriteOnlyRepository : EfCoreWriteOnlyRepository<Asset, ApplicationDbContext>, IAssetWriteOnlyRepository
{
    public AssetWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }

    public async Task<Asset> CreateAssetAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        await InsertAsync(asset, cancellationToken);
        return asset;
    }
    
    public async Task<IList<Asset>> CreateAssetAsync(IList<Asset> assets, CancellationToken cancellationToken = default)
    {
        await InsertAsync(assets, cancellationToken);
        return assets;
    }

    public async Task<Asset> UpdateAssetAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        await UpdateAsync(asset, cancellationToken);
        return asset;
    }

    public async Task<bool> DeleteAssetAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(asset, cancellationToken);
        return true;
    }
}