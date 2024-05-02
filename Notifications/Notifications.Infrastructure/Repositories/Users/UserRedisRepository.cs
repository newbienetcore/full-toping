using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.PaginatedList;
using Notifications.Application.NotificationGroups.Models;
using Notifications.Domain.Entities;
using Shared.Caching.Abstractions;

namespace Notifications.Infrastructure.Repositories.NotificationGroups;

public class UserRedisRepository : IUserRepository
{
    private readonly ICacheService _cacheService;
    public UserRedisRepository(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    

    public async Task<User> GetByIdAsync(string Id, CancellationToken cancellationToken)
    {
        var users = await _cacheService.GetAsync<List<User>>(nameof(User));
        if (users is null) return default!;
        
        var index = users.FindIndex(user => user.Id == Id);
        if (index is -1) return default!;

        return users[index];
    }

    public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var users = await _cacheService.GetAsync<List<User>>(nameof(User));
        if (users is null) return default!;
        
        var index = users.FindIndex(user => user.UserName == userName);
        if (index is -1) return default!;

        return users[index];
    }

    public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken)
    {
        var users = await _cacheService.GetAsync<List<User>>(nameof(User));
        
        if (users is null) users = new List<User>();

        entity.Id = Guid.NewGuid().ToString();
        users.Add(entity);

        await _cacheService.SetAsync<List<User>>(nameof(User), users);

        return entity;
    }
}