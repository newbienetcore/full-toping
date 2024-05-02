using FluentValidation;
using Notifications.Application.Common.Exceptions;

namespace Notifications.Application.NotificationGroups.Commands.Create;

public class CreateNotificationGroupCommandValidator : AbstractValidator<CreateNotificationGroupCommand>
{
    public CreateNotificationGroupCommandValidator()
    {
        RuleFor(a => a.Code)
            .Length(5, 50)
            .NotEmpty()
            .WithMessage(ErrorCode.CodeMinLength5MaxLength50);
        
        RuleFor(a => a.Name)
            .Length(5, 250)
            .NotEmpty()
            .WithMessage(ErrorCode.FieldIsRequired);
    }
}