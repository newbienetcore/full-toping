using SharedKernel.Libraries;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts
{
    [AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
    public class BaseAllowAnonymousQuery<TResponse> : BaseQuery<TResponse>
    {
    }
}
