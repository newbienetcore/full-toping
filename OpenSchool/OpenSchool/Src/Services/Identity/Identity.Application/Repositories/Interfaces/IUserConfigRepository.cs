using Identity.Application.Persistence;
using Identity.Domain.Entities;
using SharedKernel.Contracts.Repositories;

namespace Identity.Application.Repositories.Interfaces;

public interface IUserConfigRepository : IWriteOnlyRepository<UserConfig, Guid, IApplicationDbContext>
{
    Task<UserConfig> CreateOrUpdateAsync(UserConfig userConfig, CancellationToken cancellationToken);
    
    Task<UserConfig> GetConfigAsync(CancellationToken cancellationToken);
}