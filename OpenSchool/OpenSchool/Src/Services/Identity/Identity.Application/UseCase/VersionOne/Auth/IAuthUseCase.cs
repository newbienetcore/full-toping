using Identity.Application.DTOs.Auth;
using SharedKernel.Contracts;
using SharedKernel.Domain;

namespace Identity.Application.UseCase.VersionOne;

public interface IAuthUseCase
{
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken = default);
    
    Task<AuthResponse> SignInByPhone(SignInByPhoneDto signInByPhoneDto, CancellationToken cancellationToken = default);

    Task<bool> SignOutAsync(CancellationToken cancellationToken = default);

    Task<bool> SignOutAllDeviceAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RequestValue> GetRequestInformationAsync(CancellationToken cancellationToken = default);
    
    Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPaging(PagingRequest pagingRequest, CancellationToken cancellationToken = default);
} 