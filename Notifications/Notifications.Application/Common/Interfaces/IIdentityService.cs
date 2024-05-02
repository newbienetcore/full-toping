using Notifications.Application.Common.Models.Abstractions;
using Notifications.Domain.Entities;

namespace Notifications.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<IList<string>> GetRolesAsync(User user);
}