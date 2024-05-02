using FluentValidation;

namespace Shared.DTOs.Brands;

public class EditBrandDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class EditBrandDtoValidator : AbstractValidator<BrandDto>
{
    public EditBrandDtoValidator()
    {
        RuleFor(a => a.Name)
            .Length(5, 250)
            .NotEmpty();

        RuleFor(a => a.Description)
            .MinimumLength(5);
    }
}