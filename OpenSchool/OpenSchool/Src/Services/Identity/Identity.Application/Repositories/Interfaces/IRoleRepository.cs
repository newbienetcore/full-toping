using Identity.Application.DTOs.Role.Responses;
using Identity.Application.Persistence;
using Identity.Domain.Entities;
using SharedKernel.Contracts.Repositories;

namespace Identity.Application.Repositories.Interfaces;

public interface IRoleRepository : IWriteOnlyRepository<Role, Guid, IApplicationDbContext>
{
    Task<RoleDto> CreateRoleAsync(Role role, CancellationToken cancellationToken = default);

    Task<RoleDto> UpdateRoleAsync(Role role, CancellationToken cancellationToken = default);

    Task<bool> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}