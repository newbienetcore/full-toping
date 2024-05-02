using MediatR;
using SharedKernel.Libraries;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class BaseAllowAnonymousCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseAllowAnonymousCommand : BaseAllowAnonymousCommand<Unit>
{
}