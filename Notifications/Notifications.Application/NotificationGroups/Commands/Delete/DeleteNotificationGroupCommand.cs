using MediatR;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.Responses;
using Shared.Caching.Abstractions;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.Application.NotificationGroups.Commands.Delete;

public record DeleteNotificationGroupCommand(Guid groupId) : IRequest<ApiResponse>;

public class DeleteNotificationGroupCommandHandler : IRequestHandler<DeleteNotificationGroupCommand, ApiResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public DeleteNotificationGroupCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }
    
    public async Task<ApiResponse> Handle(DeleteNotificationGroupCommand request, CancellationToken cancellationToken)
    {
        var notificationGroup = await _context.NotificationGroups.FindAsync(new[] { request.groupId }, cancellationToken).ConfigureAwait(false);
        if(notificationGroup is null)
            throw new ApplicationException(ErrorCode.NotificationGroupNotFound, ErrorCode.NotificationGroupNotFound);

        _context.NotificationGroups.Remove(notificationGroup);

        await _context.SaveChangesAsync(cancellationToken);
        
        await _cacheService.DeleteAsync("notification-groups");

        return new ApiResponse();
    }
}