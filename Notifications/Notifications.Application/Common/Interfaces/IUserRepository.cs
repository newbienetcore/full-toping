using Notifications.Application.Common.Models.Abstractions;
using Notifications.Application.Common.Models.PaginatedList;
using Notifications.Application.NotificationGroups.Models;
using Notifications.Domain.Entities;

namespace Notifications.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string Id, CancellationToken cancellationToken);
    Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<User> CreateAsync(User entity, CancellationToken cancellationToken);
}