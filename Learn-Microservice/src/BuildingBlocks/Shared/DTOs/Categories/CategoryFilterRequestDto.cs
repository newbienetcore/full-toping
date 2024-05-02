using FluentValidation;
using Shared.DTOs.Requests;

namespace Shared.DTOs.Categories;

public class CategoryFilterRequestDto : FilterRequestDto
{
    public Guid CategoryId { get; set; }
}

public class BrandFilterRequestDtoValidator : AbstractValidator<FilterRequestDto>
{
    public BrandFilterRequestDtoValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(1).WithMessage("PageIndex at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}