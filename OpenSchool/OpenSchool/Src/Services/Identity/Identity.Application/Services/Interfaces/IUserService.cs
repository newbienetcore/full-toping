namespace Identity.Application.Services.Interfaces;

public interface IUserService
{
    Task<string> GetAvatarUrlByFileNameAsync(string fileName, Guid ownerId, CancellationToken cancellationToken);
}