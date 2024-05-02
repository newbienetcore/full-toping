using Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Application.Repositories;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Persistence;
using SharedKernel.Runtime.Exceptions;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures.Repositories;

public class EfCoreWriteOnlyRepository<TEntity,TDbContext> 
    : EfCoreReadOnlyRepository<TEntity, TDbContext>, IEfCoreWriteOnlyRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : AppDbContext
{
    
    public EfCoreWriteOnlyRepository(
        TDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider) 
        : base(dbContext, currentUser, sequenceCaching, provider)
    {

    }
    
    public IUnitOfWork UnitOfWork => _dbContext;

    #region [INSERTS]
    
    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(new List<TEntity>() { entity });
        
        await _dbSet.AddAsync(entity, cancellationToken);
        
        await ClearCacheWhenChangesAsync(null, cancellationToken);
        
        return entity;
    }

    public async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entities);
        
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        
        await ClearCacheWhenChangesAsync(null, cancellationToken);
        
        return entities;
    }
    
    #endregion

    #region [UPDATE]
    
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;
        
        TEntity exist = await _dbSet.FindAsync(entity.Id);
        _dbSet.Entry(exist).CurrentValues.SetValues(entity);
        
        await ClearCacheWhenChangesAsync(new List<object>() { entity.Id }, cancellationToken);
    }
    
    #endregion

    #region [DELETE]

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeDelete(new List<TEntity>() { entity });
        
        _dbContext.Set<TEntity>().Remove(entity);
        
        await ClearCacheWhenChangesAsync(new List<object>() { entity.Id }, cancellationToken);
    }
    
    public async Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeDelete(entities);
        
        _dbContext.Set<TEntity>().RemoveRange(entities);

        await ClearCacheWhenChangesAsync(entities.Select(x => (object)x.Id).ToList(), cancellationToken);
    }
    
    #endregion
    
    #region [PROTECTED]
    protected virtual void BeforeInsert(IEnumerable<TEntity> entities)
    {
        var batches = entities.ChunkList(1000);
        batches.ToList().ForEach(async entities =>
        {
            entities.ForEach(entity =>
            {
                entity.Id = Guid.NewGuid();
                // entity.CreatedBy = _currentUser.Context.OwnerId;
                entity.CreatedBy = Guid.NewGuid(); // Cần sửa lại chỗ này nhé
                entity.CreatedDate = DateHelper.Now;
                entity.LastModifiedDate = null;
                entity.LastModifiedBy = null;
                entity.DeletedDate = null;
                entity.DeletedBy = null;
            });

            if (batches.Count() > 1)
            {
                await Task.Delay(69);
            }
        });
        
    }

    protected virtual void BeforeUpdate(TEntity entity, TEntity oldValue)
    {
        entity.LastModifiedDate = DateHelper.Now;
        entity.LastModifiedBy = _currentUser.Context.OwnerId;
        entity.DeletedDate = null;
        entity.DeletedBy = null;
    }

    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.DeletedDate = DateHelper.Now;
            entity.DeletedBy = _currentUser.Context.OwnerId;
            entity.IsDeleted = true;
        }
    }
    protected virtual async Task ClearCacheWhenChangesAsync(List<object> ids, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        var fullRecordKey = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        tasks.Add(_sequenceCaching.DeleteAsync(fullRecordKey, cancellationToken: cancellationToken));

        if (ids is not null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
                tasks.Add(_sequenceCaching.DeleteAsync(recordByIdKey, cancellationToken: cancellationToken));
            }
        }
        
        await Task.WhenAll(tasks);
    }
    #endregion
    
}