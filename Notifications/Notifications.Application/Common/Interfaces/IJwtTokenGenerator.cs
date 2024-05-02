using Notifications.Domain.Entities;

namespace Notifications.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> Generate(User user);
}