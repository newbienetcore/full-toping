using MediatR;

namespace SharedKernel.Contracts;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>
{
}

public abstract class BaseCommand : IRequest
{
}