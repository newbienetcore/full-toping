using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class DeleteCategoryCommandHandler : BaseCommandHandler, IRequestHandler<DeleteCategoryCommand, object>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    
    public DeleteCategoryCommandHandler(
        IServiceProvider provider,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository, 
        IStringLocalizer<Resources> localizer
        ) : base(provider)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _localizer = localizer;
    }
    
    public async Task<object> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.CategoryId, out var categoryId))
        {
            throw new BadRequestException(_localizer["category_id_is_invalid"].Value);
        }
        
        var category = await _categoryReadOnlyRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            throw new BadRequestException(_localizer["category_does_not_exist_or_was_deleted"].Value);
        }
        
        var hasProducts = await _categoryReadOnlyRepository.HasProductCategoriesAsync(categoryId, cancellationToken);
        if (hasProducts)
        {
            throw new BadRequestException(_localizer["category_contains_products_and_cannot_be_deleted."].Value);
        }
        
        var isParentCategory = await _categoryReadOnlyRepository.IsParentCategoryAsync(categoryId, cancellationToken);
        if (isParentCategory)
        {
            throw new BadRequestException(_localizer["category_is_a_parent_category_and_cannot_be_deleted."].Value);
        }
        
        await _categoryWriteOnlyRepository.DeleteCategoryAsync(category, cancellationToken);
        await _categoryWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        return category.Id;
    }
    
}
