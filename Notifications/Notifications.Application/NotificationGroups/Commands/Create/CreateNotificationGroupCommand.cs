using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Common.Exceptions;
using Notifications.Application.Common.Interfaces;
using Notifications.Application.Common.Models.Responses;
using Notifications.Domain.Entities;
using Shared.Caching.Abstractions;
using Shared.Utilities;
using ApplicationException = Notifications.Application.Common.Exceptions.ApplicationException;

namespace Notifications.Application.NotificationGroups.Commands.Create;

public record CreateNotificationGroupCommand : IRequest<ApiResponse<Guid>>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string[]? Variables { get; set; }
    public string[]? SupportedUserLevel { get; set; }
    
    public Guid AppId { get; set; }
}

public class CreateNotificationGroupCommandHandler : IRequestHandler<CreateNotificationGroupCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public CreateNotificationGroupCommandHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }
    
    public async Task<ApiResponse<Guid>> Handle(CreateNotificationGroupCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request, cancellationToken);
        var notificationGroupNew = new NotificationGroup()
        {
            Code = request.Code,
            Name = request.Name,
            ParentId = request.ParentId,
            Variables = request.Variables,
            SupportedUserLevel = request.SupportedUserLevel,
            AppId = request.AppId
        };

        // notificationGroupNew.DomainEvents.Add(new NotificationGroupCompletedEvent(notificationGroupNew));
        
        await _context.NotificationGroups.AddAsync(notificationGroupNew, cancellationToken).ConfigureAwait(false);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        await _cacheService.DeleteAsync("notification-groups");
        
        return new ApiResponse<Guid>(notificationGroupNew.Id);
    }

    public async Task ValidateRequest(CreateNotificationGroupCommand request, CancellationToken cancellationToken)
    {
        var isExistNotificationGroup = await _context.NotificationGroups
            .AnyAsync(u => u.Code == request.Code || u.Code == request.Name, cancellationToken).ConfigureAwait(false);
        
        if (isExistNotificationGroup)
            throw new BadRequestException("Notification group with given code or name already exists.");

        var app = await _context.Apps.FirstOrDefaultAsync(a => a.Id == request.AppId, cancellationToken).ConfigureAwait(false);
        if(app is null)
            throw new ApplicationException(ErrorCode.ApplicationNotFound, ErrorCode.ApplicationNotFound);

        if (request.ParentId.HasValue)
        {
            var parent = await _context.NotificationGroups
                .FirstOrDefaultAsync(n => n.Id == request.ParentId.Value, cancellationToken).ConfigureAwait(false);

            if (parent is null)
                throw new ApplicationException(ErrorCode.NotificationGroupNotFound, ErrorCode.NotificationGroupNotFound);
        }
        
        if (request.Variables is not null && request.Variables.NotNullOrEmpty() && request.Variables.HasDuplicated(x => x))
            throw new BadRequestException("Variables are duplicated.");

        if (request.SupportedUserLevel is not null && request.SupportedUserLevel.NotNullOrEmpty() && request.SupportedUserLevel.HasDuplicated(x => x))
            throw new BadRequestException("SupportedUserLevel are duplicated.");
    }
}


