using Identity.Application.DTOs.Auth;
using Identity.Application.DTOs.Cpanel;
using SharedKernel.Contracts;

namespace Identity.Application.UseCase.VersionOne;

public interface ICpanelUseCase
{
    Task<CpanelAccountDto> CreateAccountAsync(CpanelCreateAccountDto cpanelCreateAccountDto, CancellationToken cancellationToken = default);
    
    Task<CpanelAccountDto> UpdateAccountAsync(Guid userId, CpanelUpdateAccountDto cpanelUpdateAccountDto, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAccountAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> CreateRoleAsync(CpanelCreateRoleDto cpanelCreateRoleDto, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> UpdateRoleAsync(CpanelUpdateRoleDto cpanelUpdateRoleDto, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteRoleAsync(CpanelUpdateRoleDto cpanelUpdateRoleDto, CancellationToken cancellationToken = default);
    
    Task<UserRoleDto> AssignOrUpdateRoleForUserAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default);

    Task<UserPermissionDto> AssignOrUpdatePermissionForUserAsync(Guid userId, List<Guid> permissionIds, CancellationToken cancellationToken = default);

    Task<RolePermissionDto> AssignOrUpdatePermissionForRoleAsync(Guid roleId, List<Guid> permissionIds, CancellationToken cancellationToken = default);
    
    Task<IPagedList<CpanelAccountDto>> GetUserPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<IPagedList<CpanelRoleDto>> GetRolePagingAsync(PagingRequest request, CancellationToken cancellationToken = default);

    Task<IPagedList<CpanelRoleDto>> GetPermissionPagingAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<CpanelAccountDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<CpanelRoleDto> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
}