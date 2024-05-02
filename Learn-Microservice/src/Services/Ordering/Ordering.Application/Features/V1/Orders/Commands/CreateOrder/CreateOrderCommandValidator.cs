using FluentValidation;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(o => o.UserName)
            .NotEmpty().WithMessage("{UserName} is required.");
    }
}