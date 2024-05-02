using System.Collections.Concurrent;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    protected bool _disposed = false;
    protected readonly TContext _dbContext;
    protected IDbContextTransaction _transaction;
    protected ConcurrentDictionary<Type, object> _repositories;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public bool SaveChanges()
    {
        return _dbContext.SaveChanges() > 0;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.SaveChangesAsync(cancellationToken)) > 0;
    }

    public void Rollback()
    {
        if (_transaction is null)
            return;

        _transaction.Rollback();
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null)
            return;
        
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
        if (_transaction is not null)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void BeginTransaction()
    {
        StartNewTransactionIfNeeded();
    }
    

    public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : EntityBase<TKey>
    {
        if (_repositories is null) _repositories = new ConcurrentDictionary<Type, object>();
        
        var typeOfEntity = typeof(TEntity);
        if (!_repositories.ContainsKey(typeOfEntity)) 
            _repositories[typeOfEntity] = new Repository<TEntity, TKey, TContext>(_dbContext);
        
        return (IRepository<TEntity, TKey>)_repositories[typeOfEntity];
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            if (_repositories != null)
                _repositories.Clear();

            if (_transaction != null)
                _transaction.Dispose();

            _transaction = null;

            _dbContext.Dispose();
        }

        _disposed = false;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
    
    #region [PRIVATE METHODS]

    private void StartNewTransactionIfNeeded()
    {
        if (_transaction is null)
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }
    }
    
    #endregion
}