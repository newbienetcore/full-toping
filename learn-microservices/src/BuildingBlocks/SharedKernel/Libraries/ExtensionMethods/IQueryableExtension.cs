using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;

namespace SharedKernel.Libraries;

public static class IQueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull((object) source, nameof (source));
        ArgumentNullException.ThrowIfNull((object) predicate, nameof (predicate));

        return condition ? source.Where(predicate) : source;
    }
    
    public static async Task<TSource?> FirstOrDefaultIfAsync<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull((object) source, nameof (source));
        ArgumentNullException.ThrowIfNull((object) predicate, nameof (predicate));
        
        return condition ? await source.FirstOrDefaultAsync(predicate, cancellationToken) : default!;
    }
    
    public static IQueryable<TEntity> ApplySorting<TEntity>(this IQueryable<TEntity> source, List<SortModel> sortModels)
    {
        ArgumentNullException.ThrowIfNull((object) source, nameof (source));

        if (!sortModels.Any()) return source;
        
        IOrderedQueryable<TEntity>? orderedQuery = null;

        foreach (var sortModel in sortModels)
        {
            var propertyInfo = typeof(TEntity).GetProperty(sortModel.FieldName);
            if (propertyInfo == null)
            {
                ArgumentNullException.ThrowIfNull(propertyInfo, nameof (propertyInfo));
            }
            
            var parameter = Expression.Parameter(typeof(TEntity), sortModel.FieldName);
            var propertyExpression = Expression.Property(parameter, propertyInfo);
            var lambdaExpression = Expression.Lambda<Func<TEntity, dynamic>>(propertyExpression, parameter);
            
            if (sortModel.SortAscending)
            {
                orderedQuery = orderedQuery == null
                    ? source.OrderBy(lambdaExpression)
                    : orderedQuery.ThenBy(lambdaExpression);
            }
            else
            {
                orderedQuery = orderedQuery == null
                    ? source.OrderByDescending(lambdaExpression)
                    : orderedQuery.ThenByDescending(lambdaExpression);
            }
        }
        
        return orderedQuery ?? source;
    }
}