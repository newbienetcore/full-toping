using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class CreateCategoryCommandHandler : BaseCommandHandler, IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    
    public CreateCategoryCommandHandler(IServiceProvider provider, 
        ICategoryReadOnlyRepository categoryReadOnlyRepository, 
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        IStringLocalizer<Resources> localizer, 
        IMapper mapper) : base(provider)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        request.Alias = request.Name.ToUnsignString();

        await ValidateDuplicateAsync( request.Name, cancellationToken);
        
        var parent = await GetAndValidateParentAsync(request.ParentId, cancellationToken);
        
        var entity = _mapper.Map<Category>(request);
        
        var category = await _categoryWriteOnlyRepository.CreateCategoryAsync(entity, cancellationToken);
        UpdateLevelAndPath(category, parent);
        await _categoryWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return categoryDto;
    }
    
    private async Task<Category?> GetAndValidateParentAsync(Guid? parentId, CancellationToken cancellationToken)
    {
        if (parentId == null || parentId == Guid.Empty)
            return null;

        var parent = await _categoryReadOnlyRepository.GetCategoryByIdAsync(parentId.Value, cancellationToken);
        if (parent is null)
        {
            throw new BadRequestException(_localizer["category_parent_invalid"].Value);
        }

        return parent;
    }

    private async Task ValidateDuplicateAsync(string name, CancellationToken cancellationToken)
    {
        var codeDuplicate = await _categoryReadOnlyRepository.IsDuplicate(null, name, cancellationToken);
        if (!string.IsNullOrWhiteSpace(codeDuplicate))
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }
    }

    private void UpdateLevelAndPath(Category category, Category? parent)
    {
        if (parent == null)
        {
            category.Level = 1;
            category.Path = $"/{category.Id}";
        }
        else
        {
            category.Level = parent.Level + 1;
            category.Path = $"{parent.Path}/{category.Alias}";
        }
    }
}