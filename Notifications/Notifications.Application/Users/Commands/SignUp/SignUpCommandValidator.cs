using FluentValidation;

namespace Notifications.Application.Users.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("Do Chi Hung");

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .NotEmpty();
        
        RuleFor(x => x.CountryCode)
            .MaximumLength(50)
            .NotEmpty();
    }
}
