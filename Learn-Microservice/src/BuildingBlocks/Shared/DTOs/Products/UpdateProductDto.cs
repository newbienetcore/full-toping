using FluentValidation;

namespace Shared.DTOs.Products;

public class UpdateProductDto : CreateOrUpdateProductDto
{
    
}

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
       
        RuleFor(a => a.Name)
            .Length(5, 250)
            .NotEmpty();

        RuleFor(a => a.Summary)
            .MinimumLength(5)
            .NotEmpty();

        RuleFor(a => a.Price)
            .GreaterThanOrEqualTo(0);
        
        RuleForEach(a => a.ProductImages)
            .Must(url => (bool)ValidateImageUrl(url))
            .When(a => a.ProductImages != null && a.ProductImages.Any());
    }
    
    private bool ValidateImageUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        string imagePath = Path.Combine("wwwroot", "uploads", url);

        return File.Exists(imagePath);
    }
}