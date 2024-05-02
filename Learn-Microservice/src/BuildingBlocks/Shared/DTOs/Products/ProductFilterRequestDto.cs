using FluentValidation;
using Shared.DTOs.Requests;

namespace Shared.DTOs.Products;

public class ProductFilterRequestDto : FilterRequestDto
{
    public Guid BrandId { get; set; } = Guid.Empty;
    public Guid CategoryId { get; set; } = Guid.Empty;
    public decimal? FromPrice { get; set; }
    public decimal? ToPrice { get; set; }
}

public class ProductFilterRequestDtoValidator : AbstractValidator<ProductFilterRequestDto>
{
    public ProductFilterRequestDtoValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(1).WithMessage("PageIndex at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        
        RuleFor(x => x.FromPrice)
            .Must(price => price is null || price >= 0);

        RuleFor(x => x.ToPrice)
            .Must(price => price is null || price >= 0);

        RuleFor(x => x)
            .Must(HaveValidPriceRange)
            .When(x => x.FromPrice.HasValue && x.ToPrice.HasValue);
    }

    private bool HaveValidPriceRange(ProductFilterRequestDto model)
    {
        return model.FromPrice <= model.ToPrice;
    }
}