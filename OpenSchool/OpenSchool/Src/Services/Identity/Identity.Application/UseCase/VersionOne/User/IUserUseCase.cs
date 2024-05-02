using Identity.Application.DTOs.Cpanel;
using Identity.Application.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.UseCase.VersionOne;

public interface IUserUseCase
{
    Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserDto> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<AvatarDto> SetAvatarAsync(IFormFile avatar, CancellationToken cancellationToken = default);
    Task<bool> RemoveAvatarAsync(Guid userId, CancellationToken cancellationToken = default);
}