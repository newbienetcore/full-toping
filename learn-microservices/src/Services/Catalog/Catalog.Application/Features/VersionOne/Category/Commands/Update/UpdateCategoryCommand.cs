using AutoMapper;
using Catalog.Application.Mappings;
using Catalog.Application.Properties;
using Catalog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

public class UpdateCategoryCommand : BaseUpdateCommand<Unit>, IMapFrom<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Alias  { get; set; }
    public string? Description { get; set; }
    public int? Level { get; set; }
    public string FileName { get; set; }
    public int OrderNumber { get; set; }
    public bool Status { get; set; }
    public Guid? ParentId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateSupplierCommand, Supplier>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IStringLocalizer<Resources> localizer)
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