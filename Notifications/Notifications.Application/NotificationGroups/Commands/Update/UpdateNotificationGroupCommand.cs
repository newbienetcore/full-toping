using Duende.IdentityServer.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.Responses;
using Notifications.Domain.Entities;
using Shared.Caching.Abstractions;
using Shared.Utilities;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.Application.NotificationGroups.Commands.Update;

public record UpdateNotificationGroupCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string[]? Variables { get; set; }
    public string[]? SupportedUserLevel { get; set; }
    public Guid AppId { get; set; }
}

public class UpdateNotificationGroupCommandHandler : IRequestHandler<UpdateNotificationGroupCommand, ApiResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public UpdateNotificationGroupCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }
    
    public async Task<ApiResponse> Handle(UpdateNotificationGroupCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request, cancellationToken);
        
        var notificationGroup = await _context.NotificationGroups.FindAsync(new object[] { request.Id }, cancellationToken)
            .ConfigureAwait(false);
        if (notificationGroup is null)
            throw new ApplicationException(ErrorCode.ApplicationNotFound, ErrorCode.ApplicationNotFound);

        notificationGroup.Code = request.Code;
        notificationGroup.Name = request.Name;
        notificationGroup.ParentId = request.ParentId;
        notificationGroup.Variables = request.Variables;
        notificationGroup.SupportedUserLevel = request.SupportedUserLevel;
        notificationGroup.AppId = request.AppId;

        _context.NotificationGroups.Update(notificationGroup);
        await _context.SaveChangesAsync(cancellationToken);
        
        await _cacheService.DeleteAsync("notification-groups");
        
        return new ApiResponse();
    }

    private async Task ValidateRequest(UpdateNotificationGroupCommand request, CancellationToken cancellationToken)
    {
        var notificationGroupDuplicate = await _context.NotificationGroups
            .FirstOrDefaultAsync(
                u => (u.Code.Contains(request.Code) || u.Code.Contains(request.Name)) && u.Id != request.Id,
                cancellationToken).ConfigureAwait(false);

        if (notificationGroupDuplicate is not null)
            throw new BadRequestException("The notification group is duplicate.");

        var app = await _context.Apps.FirstOrDefaultAsync(a => a.Id == request.AppId, cancellationToken)
            .ConfigureAwait(false);
        if (app is null)
            throw new ApplicationException(ErrorCode.ApplicationNotFound, ErrorCode.ApplicationNotFound);

        if (request.ParentId.HasValue)
        {
            var parent = await _context.NotificationGroups
                .FirstOrDefaultAsync(n => n.Id == request.ParentId.Value, cancellationToken).ConfigureAwait(false);

            if (parent is null)
                throw new ApplicationException(ErrorCode.ApplicationNotFound, ErrorCode.ApplicationNotFound);
        }

        if (request.Variables is not null && request.Variables.NotNullOrEmpty() && request.Variables.HasDuplicated(x => x))
            throw new BadRequestException("Variables are duplicated.");

        if (request.SupportedUserLevel is not null && request.SupportedUserLevel.NotNullOrEmpty() && request.SupportedUserLevel.HasDuplicated(x => x))
            throw new BadRequestException("SupportedUserLevel are duplicated.");
    }
}
