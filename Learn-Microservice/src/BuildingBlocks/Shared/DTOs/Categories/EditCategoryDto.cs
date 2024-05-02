using FluentValidation;

namespace Shared.DTOs.Categories;

public class EditCategoryDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class EditCategoryDtoValidator : AbstractValidator<EditCategoryDto>
{
    public EditCategoryDtoValidator()
    {
        RuleFor(a => a.Name)
            .Length(5, 250)
            .NotEmpty();

        RuleFor(a => a.Description)
            .MinimumLength(5);
    }
}