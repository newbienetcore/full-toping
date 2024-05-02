using FluentValidation;
using Shared.DTOs.Requests;

namespace Shared.DTOs.Brands;

public class BrandFilterRequestDto : FilterRequestDto
{
    
}

public class BrandFilterRequestDtoValidator : AbstractValidator<BrandFilterRequestDto>
{
    public BrandFilterRequestDtoValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(1).WithMessage("PageIndex at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}