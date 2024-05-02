using SharedKernel.Domain;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public interface IAuthService
{
    bool CheckPermission(ActionExponent exponent);

    bool CheckPermission(ActionExponent[] exponents);

    Task<string> GenerateAccessTokenAsync(TokenUser token, CancellationToken cancellationToken = default);

    string GenerateRefreshToken();

    Task RevokeAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default);
    
    Task<List<string>> RevokeAllAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> CheckRefreshTokenAsync(string value, Guid ownerId, CancellationToken cancellationToken = default);

    Task<bool> IsNewLoginAddressAsync(RequestValue requestValue, CancellationToken cancellationToken = default);

    Task<RequestValue> GetRequestValueAsync(CancellationToken cancellationToken = default);

    string GenerateOtp();
    
    

}