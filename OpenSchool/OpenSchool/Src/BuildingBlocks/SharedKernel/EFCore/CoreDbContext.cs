using EFCore.BulkExtensions;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace SharedKernel.EFCore;

public class CoreDbContext : DbContext, ICoreDbContext
{
    public CoreDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    #region Implements

    public async Task BulkInsertAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : EntityAuditBase
    {
        ApplyAuditFieldsToModifiedEntities();
        await DbContextBulkExtensions.BulkInsertAsync(this, entities, cancellationToken: cancellationToken);
    }

    public async Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
    {
        ApplyAuditFieldsToModifiedEntities();
        await DbContextBulkExtensions.BulkUpdateAsync(this, entities, cancellationToken: cancellationToken);
    }

    public async Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
    {
        ApplyAuditFieldsToModifiedEntities();
        if (typeof(TEntity).HasInterface<ISoftDelete>())
        {
            await DbContextBulkExtensions.BulkUpdateAsync(this, entities, cancellationToken: cancellationToken);
        }
        else
        {
            await DbContextBulkExtensions.BulkDeleteAsync(this, entities, cancellationToken: cancellationToken);
        }
    }
    
    #endregion

    #region Implement UnitOfWork

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (this.Database.CurrentTransaction is null)
        {
            return;
        }
            
        await this.Database.RollbackTransactionAsync(cancellationToken);
    }
        

    public virtual void BeginTransaction()
    {
        if (this.Database.CurrentTransaction is null)
        {
            this.Database.BeginTransaction();
        }
    }
        
    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFieldsToModifiedEntities();
        
        await this.SaveChangesAsync(cancellationToken);
            
        if (this.Database.CurrentTransaction is not null)
        {
            await this.Database.CommitTransactionAsync(cancellationToken);
        }
    }

    #endregion

    #region Private methods

    private void ApplyAuditFieldsToModifiedEntities()
    {
        var currentUser = this.GetService<ICurrentUser>();
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.CreatedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.CreatedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Modified: 
                {
                    Entry(entry.Entity).Property("Id").IsModified = false;
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.LastModifiedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.LastModifiedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Deleted:
                {
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        Entry(entry.Entity).Property("Id").IsModified = false;
                        softDelete.DeletedBy = currentUser.Context.OwnerId;
                        softDelete.DeletedDate = DateHelper.Now;
                        softDelete.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
                }
            }
        }
        
    }
    #endregion
}