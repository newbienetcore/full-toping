
using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Common;

public class RepositoryQueryBase<TEntity, TKey, TContext> : IRepositoryQueryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryQueryBase(TContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
        => Query(predicate, conditionPredicates, orderBy, include, disableTracking);
    
    public async Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true, 
        CancellationToken cancellationToken = default)
        => await Query(predicate, conditionPredicates, orderBy, include, disableTracking).ToListAsync(cancellationToken);

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
        => await Query(predicate, conditionPredicates, orderBy, include, disableTracking).FirstOrDefaultAsync(cancellationToken);
    
    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await _dbSet.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, CancellationToken cancellationToken = default)
        => await include(_dbSet.Where(x => x.Id.Equals(id))).FirstOrDefaultAsync(cancellationToken);


    #region [PRIVATE METHOD]
    private IQueryable<TEntity> Query(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        if (conditionPredicates is not null && conditionPredicates.Any())
        {
            
            query = query.Where(
                entity => conditionPredicates.All(cp => !cp.condition || cp.predicate.Compile()(entity)));
        }

        if (orderBy is not null)
            query = orderBy(query);

        return query;
    }
    #endregion [PRIVATE METHOD]
    
}