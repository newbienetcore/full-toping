using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.PaginatedList;
using Notifications.Application.NotificationGroups.Models;
using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Repositories.NotificationGroups;

public class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _dbContext;
    public UserRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByIdAsync(string Id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == Id, cancellationToken).ConfigureAwait(false);
        
    }

    public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}