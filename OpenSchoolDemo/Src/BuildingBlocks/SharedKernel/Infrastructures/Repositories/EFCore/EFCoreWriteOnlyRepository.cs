using Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.EFCore;
using SharedKernel.Libraries;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures;


public class EFCoreWriteOnlyRepository<TEntity, TKey, TDbContext>
    : EFCoreReadOnlyRepository<TEntity, TKey, TDbContext>, IEFCoreWriteOnlyRepository<TEntity, TKey, TDbContext>
    where TEntity : EntityBase<TKey>
    where TDbContext : CoreDbContext
{
    public EFCoreWriteOnlyRepository(
        TDbContext context,
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(context, currentUser, sequenceCaching)
    {
    }

    public IUnitOfWork UnitOfWork => _context;

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(new List<TEntity>() { entity });
        await _dbSet.AddAsync(entity, cancellationToken);
        
        return entity;
    }

    public virtual async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
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
        BeforeDelete(new List<TEntity>() { entity });
        
        _context.Set<TEntity>().Remove(entity);
        
        await ClearCacheWhenChangesAsync(new List<TKey>() { entity.Id }, cancellationToken);
    }

    public virtual async Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeDelete(entities);
        
        _context.Set<TEntity>().RemoveRange(entities);

        await ClearCacheWhenChangesAsync(entities.Select(x => x.Id).ToList(), cancellationToken);
    }

    protected virtual void BeforeInsert(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is IDateTracking dateTracking)
            {
                dateTracking.CreatedDate = DateHelper.Now;
                dateTracking.LastModifiedDate = null;
            }

            if (entity is IUserTracking userTracking)
            {
                userTracking.CreatedBy = _currentUser.Context.OwnerId;
                userTracking.LastModifiedBy = null;
            }

            if (entity is ISoftDelete softDelete)
            {
                softDelete.DeletedDate = null;
                softDelete.DeletedBy = null;
                softDelete.IsDeleted = false;
            }

            if (entity is IPersonalizeEntity personalizeEntity)
            {
                personalizeEntity.OwnerId = _currentUser.Context.OwnerId;
            }
        }
    }

    protected virtual void BeforeUpdate(TEntity entity)
    {
        if (entity is IDateTracking dateTracking)
        {
            dateTracking.LastModifiedDate =  DateHelper.Now;
        }

        if (entity is IUserTracking userTracking)
        {
            userTracking.LastModifiedBy = _currentUser.Context.OwnerId;
        }
    }
    
    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is ISoftDelete softDelete)
            {
                softDelete.DeletedDate = null;
                softDelete.DeletedBy = null;
                softDelete.IsDeleted = false;
            }
        }
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