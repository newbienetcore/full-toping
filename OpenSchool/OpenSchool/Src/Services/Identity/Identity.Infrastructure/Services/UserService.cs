using Identity.Application.Services.Interfaces;

namespace Identity.Infrastructure.Services;

public class UserService : IUserService
{
    public async Task<string> GetAvatarUrlByFileNameAsync(string fileName, Guid ownerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}