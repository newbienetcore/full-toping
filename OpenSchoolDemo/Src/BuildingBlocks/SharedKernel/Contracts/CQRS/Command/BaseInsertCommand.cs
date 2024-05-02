using MediatR;
using SharedKernel.Libraries;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public class BaseInsertCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseInsertCommand : BaseInsertCommand<Unit>
{
}