using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;


namespace Infrastructure.Common;

public class Repository<TEntity, TKey, TContext> : RepositoryQueryBase<TEntity, TKey, TContext>, IRepository<TEntity, TKey> 
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    
    public Repository(TContext dbContext) : base(dbContext)
    {
        
    } 
    
    public TKey Insert(TEntity entity)
    {
        _dbSet.Add(entity);
        return entity.Id;
    }

    public IList<TKey> Insert(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        return entities.Select(e => e.Id).ToList();
    }

    public async Task<TKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task<IList<TKey>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        return entities.Select(e => e.Id).ToList();
    }

    public void Update(TEntity entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;
        TEntity exits = _dbSet.Find(entity.Id);
        _dbContext.Entry(exits).CurrentValues.SetValues(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public bool SaveChanges() => _dbContext.SaveChanges() > 0;
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
     =>  await _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task EndTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}