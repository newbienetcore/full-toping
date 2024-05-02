using Catalog.Application.DTOs;
using Catalog.Application.Mappings;
using Catalog.Application.Properties;
using Catalog.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.Category })]
public class CreateCategoryCommand : BaseInsertCommand<CategoryDto>, IMapFrom<Category>
{
    public string Name { get; set; }
    public string? Alias  { get; set; }
    public string? Description { get; set; }
    public int? Level { get; set; }
    public string FileName { get; set; }
    public int OrderNumber { get; set; }
    public bool Status { get; set; } = true;
    public Guid? ParentId { get; set; }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IStringLocalizer<Resources> localizer)
    {
        // Tên nhà cung cấp
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["category_name_required"].Value)
            .MaximumLength(255).WithMessage(localizer["category_name_max_length_255"].Value);
        
        RuleFor(e => e.FileName)
            .NotEmpty()
            .WithMessage(localizer["category_image_required"].Value)
            .MaximumLength(255)
            .WithMessage(localizer["category_image_max_length_255"].Value);
        
        RuleFor(e => e.OrderNumber)
            .GreaterThan(0)
            .WithMessage(localizer["category_order_number_must_be_greater_than_0"].Value);
        
        RuleFor(e => e.ParentId)
            .Must(e => e != Guid.Empty)
            .When(e => e.ParentId.HasValue)
            .WithMessage(localizer["parent_category_id_is_invalid"].Value);
    }
}