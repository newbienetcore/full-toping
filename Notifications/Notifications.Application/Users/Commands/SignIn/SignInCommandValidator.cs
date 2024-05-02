using FluentValidation;
using Notifications.Application.Common.Exceptions;

namespace Notifications.Application.Users.Commands.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage(ErrorCode.FieldIsRequired);

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage(ErrorCode.FieldIsRequired);

    }
}
