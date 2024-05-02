using Identity.Application.DTOs.Auth;
using Identity.Domain.Entities;
using SharedKernel.Contracts;
using SharedKernel.Domain;
using SharedKernel.UnitOfWork;

namespace Identity.Application.Repositories.Interfaces;

public interface IAuthRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<TokenUser?> GetTokenUserByIdentityAsync(string username, CancellationToken cancellationToken = default);

    Task<TokenUser?> GetTokenUserByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    
    Task<bool> CheckRefreshTokenAsync(string value, Guid userId, CancellationToken cancellationToken = default);

    Task CreateOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task RemoveRefreshTokenAsync(CancellationToken cancellationToken = default);
    
    Task RemoveRefreshTokenAsync(List<string> accessTokens, CancellationToken cancellationToken = default);

    Task CreateOtpAsync(OTP otp, CancellationToken cancellationToken = default);
    
    Task UpdateOtpAsync(OTP otp, CancellationToken cancellationToken = default);
    
    Task UsedOtpAsync(OTP otp, CancellationToken cancellationToken = default);

    Task<OTP?> GetUnexpiredOtpAsync(Guid ownerId, string otp, CancellationToken cancellationToken = default);
    
    Task<bool> VerifySecretKeyAsync(string secretKey, CancellationToken cancellationToken = default);

    Task<bool> CheckSignInHistoryAsync(RequestValue requestValue, CancellationToken cancellationToken = default);

    Task<IPagedList<SignInHistoryDto>> GetSignInHistoryPagingAsync(PagingRequest pagingRequest, CancellationToken cancellationToken = default);

}