using AutoMapper;
using Catalog.Application.Services;
using MediatR;
using SharedKernel.Application;

namespace Catalog.Application.Features.VersionOne;

public class ClearAllCachingCommandHandler : BaseQueryHandler, IRequestHandler<ClearAllCachingCommand, bool>
{
    private readonly ICachingService _cachingService;
    public ClearAllCachingCommandHandler(IMapper mapper, ICachingService cachingService) : base(mapper)
    {
        _cachingService = cachingService;
    }

    public async Task<bool> Handle(ClearAllCachingCommand request, CancellationToken cancellationToken)
    {
        return await _cachingService.ClearAllCachingAsync(cancellationToken);
    }
}