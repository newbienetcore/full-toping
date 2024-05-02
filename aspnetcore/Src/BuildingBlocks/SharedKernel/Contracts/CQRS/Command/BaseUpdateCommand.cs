using MediatR;

namespace SharedKernel.Contracts;

public class BaseUpdateCommand<TResponse> : BaseCommand<TResponse>
{
}

public class BaseUpdateCommand : BaseUpdateCommand<Unit>
{
}