using Identity.Application.DTOs.Cpanel;
using Identity.Application.DTOs.Role.Responses;
using Identity.Application.DTOs.User;
using Identity.Domain.Entities;
using SharedKernel.Contracts;

namespace Identity.Application.Repositories.Interfaces;

public interface ICpanelRepository
{
    Task AssignRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default);
    
    void RevokeRoleForUserAsync(List<UserRole> userRoles, CancellationToken cancellationToken = default);

    Task AssignPermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default);

    void RevokePermissionForRoleAsync(List<RolePermission> rolePermissions, CancellationToken cancellationToken = default);

    Task AssignPermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default);

    void RevokePermissionForUserAsync(List<UserPermission> userPermissions, CancellationToken cancellationToken = default);
    
    Task<List<CpanelAccountDto>> GetAccountsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<IPagedList<CpanelAccountDto>> GetAccountsPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<List<CpanelRoleDto>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<List<CpanelPermissionDto>> GetPermissionByExponentsAsync(List<ActionExponent> exponents, CancellationToken cancellationToken = default);

    Task<List<CpanelPermissionDto>> GetPermissionByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);

}