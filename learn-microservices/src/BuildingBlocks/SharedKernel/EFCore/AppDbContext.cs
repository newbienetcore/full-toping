using System.Reflection;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace SharedKernel.Persistence
{
    public abstract class AppDbContext : DbContext, IAppDbContext
    {
        #region Declare + Constructor
        
        public AppDbContext(DbContextOptions options) : base(options)
        {
           
        }
        
        #endregion [CONSTRUCTOR]
        
        #region Implementation
        
        public virtual async Task BulkInsertEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await this.BulkInsertAsync(entities, cancellationToken: cancellationToken);
        }
        
        public virtual async Task BulkUpdateEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await this.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
        }
        
        public virtual async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IAuditable)))
            {
                await this.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
            }
            else
            {
                await this.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
            }
        }
        
        public virtual async Task BulkCommitAsync(CancellationToken cancellationToken = default)
        {
            await this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        }
        
        #endregion

        #region Implementation UnitOfWork
        
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
            await this.SaveChangesAsync(cancellationToken);
            
            if (this.Database.CurrentTransaction is not null)
            {
                await this.Database.CommitTransactionAsync(cancellationToken);
            }
        }
        
        #endregion
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
    }
}