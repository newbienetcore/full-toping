using MediatR;
using SharedKernel.Libraries;

namespace SharedKernel.Contracts
{
    [AuthorizationRequest]
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
    }
}
