using Notifications.Domain.Entities;

namespace Notifications.Application.Users.Models;

public record AuthenticationResult
(
    User User,
    string Token
);