using Identity.Application.DTOs.User;
using Identity.Application.Persistence;
using Identity.Domain.Entities;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;

namespace Identity.Application.Repositories.Interfaces;

public interface IUserRepository : IWriteOnlyRepository<User, Guid, IApplicationDbContext>
{
    Task<UserDto> GetUserInformationAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default);
    
    Task<UserDto> CreateUserAsync(User user,CancellationToken cancellationToken = default);
    
    Task<UserDto> UpdateUserAsync(User user,CancellationToken cancellationToken = default);
    
    Task<bool> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}