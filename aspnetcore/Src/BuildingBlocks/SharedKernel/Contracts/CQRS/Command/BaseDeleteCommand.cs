using MediatR;
using SharedKernel.Libraries;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public class BaseDeleteCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseDeleteCommand : BaseDeleteCommand<Unit>
{
}