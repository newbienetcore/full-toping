namespace SharedKernel.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    
        Task RollbackAsync(CancellationToken cancellationToken = default);
    
        void BeginTransaction();
    }
}
