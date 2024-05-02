using System.Linq.Expressions;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore.Query;

namespace Contracts.Common.Interfaces;

public interface IRepositoryQueryBase<TEntity, TKey, TContext> 
    where TEntity : EntityBase<TKey>
{
    IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true);

    Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(TKey id, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        CancellationToken cancellationToken = default);
}

public interface IRepositoryQueryBase<TEntity, TKey> 
    where TEntity : EntityBase<TKey>
{
    IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true);

    Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> conditionPredicates = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id, 
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, 
        CancellationToken cancellationToken = default);
}