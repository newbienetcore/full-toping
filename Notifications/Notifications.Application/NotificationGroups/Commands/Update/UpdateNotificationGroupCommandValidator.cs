using FluentValidation;
using Notifications.Application.Common.Exceptions;

namespace Notifications.Application.NotificationGroups.Commands.Update;

public class UpdateNotificationGroupCommandValidator : AbstractValidator<UpdateNotificationGroupCommand>
{
    public UpdateNotificationGroupCommandValidator()
    {
        RuleFor(a => a.Code)
            .Length(5, 50)
            .NotEmpty()
            .WithMessage(ErrorCode.CodeMinLength5MaxLength50);

        RuleFor(a => a.Name)
            .MaximumLength(250)
            .NotEmpty()
            .WithMessage(ErrorCode.FieldIsRequired);
    }
}