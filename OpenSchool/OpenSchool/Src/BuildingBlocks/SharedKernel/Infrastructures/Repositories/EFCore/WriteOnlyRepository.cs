using Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.EFCore;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures;


public class WriteOnlyRepository<TEntity, TKey, TDbContext>
    : ReadOnlyRepository<TEntity, TKey, TDbContext>, IWriteOnlyRepository<TEntity, TKey, TDbContext>
    where TEntity : EntityBase<TKey>
    where TDbContext : CoreDbContext
{
    public WriteOnlyRepository(
        TDbContext context,
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(context, currentUser, sequenceCaching)
    {
    }

    public IUnitOfWork UnitOfWork => _context;

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        
        return entity;
    }

    public virtual async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        
        return entities;
    }
    

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged) return;

        TEntity exist = _context.Set<TEntity>().Find(entity.Id);
        _context.Entry(exist).CurrentValues.SetValues(entity);
        
        await ClearCacheWhenChangesAsync(new List<TKey>() { entity.Id }, cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        
        await ClearCacheWhenChangesAsync(new List<TKey>() { entity.Id }, cancellationToken);
    }

    public virtual async Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().RemoveRange(entities);

        await ClearCacheWhenChangesAsync(entities.Select(x => x.Id).ToList(), cancellationToken);
    }
    
    protected virtual async Task ClearCacheWhenChangesAsync(List<TKey> ids, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        var fullRecordKey = _isSystemTable ? BaseCacheKeys.GetSystemFullRecordsKey(_tableName) : BaseCacheKeys.GetFullRecordsKey(_tableName, _currentUser.Context.OwnerId);
        tasks.Add(_sequenceCaching.DeleteAsync(fullRecordKey));

        if (ids != null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = _isSystemTable ? BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id) : BaseCacheKeys.GetRecordByIdKey(_tableName, id, _currentUser.Context.OwnerId);
                tasks.Add(_sequenceCaching.DeleteAsync(recordByIdKey));
            }
        }
        
        await Task.WhenAll(tasks);
    }
    
}