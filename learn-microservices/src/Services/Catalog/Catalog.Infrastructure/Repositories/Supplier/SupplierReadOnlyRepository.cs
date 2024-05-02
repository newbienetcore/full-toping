using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using Caching;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Libraries;

namespace Catalog.Infrastructure.Repositories;

public class SupplierReadOnlyRepository : EfCoreReadOnlyRepository<Supplier, ApplicationDbContext>, ISupplierReadOnlyRepository
{
    public SupplierReadOnlyRepository(
        ApplicationDbContext dbContext,
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching,
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
        
    }

    public async Task<IList<Supplier>> GetListSupplierByIdsAsync(IList<Guid> supplierIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(e => supplierIds.Contains(e.Id)).ToListAsync(cancellationToken);
    }

    public async Task<string> IsDuplicate(Guid? id, string email, string phone, string name, CancellationToken cancellationToken = default)
    {
        var duplicateSupplier = await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(e => (id == null || e.Id != id) && (e.Phone == phone || e.Email == email || e.Name == name), cancellationToken);

        if (duplicateSupplier is null)
        {
            return string.Empty;
        }
        
        if (duplicateSupplier.Name == name)
        {
            return "supplier_is_duplicate_name";
        }
        
        if (duplicateSupplier.Email == email)
        {
            return "supplier_is_duplicate_email";
        }
        
        if (duplicateSupplier.Phone == phone)
        {
            return "supplier_is_duplicate_phone";
        }
        
        return string.Empty;
    }

    public async Task<IPagedList<SupplierDto>> GetPagingResultAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var mapper = _provider.GetRequiredService<IMapper>();
        
        var result = await _dbSet
            .WhereIf(!string.IsNullOrEmpty(request.SearchString),
                e => e.Name.Contains(request.SearchString) || e.Email.Contains(request.SearchString) || e.Description.Contains(request.SearchString))
            .ApplySorting(request.Sorts)
            .AsNoTracking()
            .ToPagedListAsync<Supplier, SupplierDto>(
                mapper,
                request.Page,
                request.Size,
                request.IndexFrom,
                cancellationToken);

        return result;
    }

    public async Task<Supplier?> GetSupplierByAliasWithCachingAsync(string alias, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, alias);
        
        var cacheResult = await _sequenceCaching.GetAsync<Supplier>(key, cancellationToken: cancellationToken);
        if (cacheResult is not null)
        {
            return cacheResult;
        }
        
        var supplier = await _dbSet.FirstOrDefaultIfAsync(!string.IsNullOrEmpty(alias), e => e.Alias == alias, cancellationToken);

        if (supplier is not null)
        {
            await _sequenceCaching.SetAsync(key, supplier, cancellationToken: cancellationToken);
        }

        return supplier;
    }
}