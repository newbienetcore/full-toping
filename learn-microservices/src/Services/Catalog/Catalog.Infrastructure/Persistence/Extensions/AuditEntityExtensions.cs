using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Extensions;

public static class AuditEntityExtensions
{
    public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
    {
        SetSoftDeleteFilterMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder });
    }
    
    static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(AuditEntityExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(t => t.IsGenericMethod && t.Name == nameof(SetSoftDeleteFilter));

    public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder) where TEntity : class, IBaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
    }
}