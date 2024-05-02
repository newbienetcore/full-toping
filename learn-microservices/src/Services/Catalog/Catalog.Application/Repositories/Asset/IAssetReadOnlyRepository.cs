using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface IAssetReadOnlyRepository : IEfCoreReadOnlyRepository<Asset, IApplicationDbContext>
{
    Task<AssetDto?> GetAssetByIdAsync(Guid assetId, CancellationToken cancellationToken = default);
    Task<AssetDto?> GetAssetByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    Task<IPagedList<AssetDto>> GetPagingResultAsync(PagingRequest request, CancellationToken cancellationToken = default);
}