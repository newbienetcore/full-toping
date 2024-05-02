using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.Responses;
using Notifications.Application.NotificationGroups.Models;
using Shared.Caching.Abstractions;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.Application.NotificationGroups.Queries.Get;

public record GetNotificationGroupQuery(Guid groupId) : IRequest<ApiResponse<NotificationGroupDto>>;

public class GetNotificationGroupQueryHandler : IRequestHandler<GetNotificationGroupQuery, ApiResponse<NotificationGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public GetNotificationGroupQueryHandler(IApplicationDbContext context, ICacheService cacheService, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<ApiResponse<NotificationGroupDto>> Handle(GetNotificationGroupQuery request, CancellationToken cancellationToken)
    {
        // var cacheData = _cacheService.GetData<PaginatedList<NotificationGroupDto>>("notification-groups");
        // var notificationGroupCache = cacheData.Items.FirstOrDefault(x => x.Id == request.groupId);
        // if (notificationGroupCache is not null)
        //     return notificationGroupCache;
        
        var notificationGroup =
            await _context.NotificationGroups.Where(n => n.Id == request.groupId)
                .Include(n => n.App)
                .ProjectTo<NotificationGroupDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

        if (notificationGroup is null)
            throw new ApplicationException(ErrorCode.NotificationGroupNotFound, ErrorCode.NotificationGroupNotFound);

        return new ApiResponse<NotificationGroupDto>(notificationGroup);
    }
}