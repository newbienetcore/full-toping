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

public class UpdateCategoryCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IServiceProvider provider,
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

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        request.Alias = request.Name.ToUnsignString();
        
        var codeDuplicate = await _categoryReadOnlyRepository.IsDuplicate(request.Id, request.Name, cancellationToken);
        if (!string.IsNullOrWhiteSpace(codeDuplicate))
        {
            throw new BadRequestException(_localizer[codeDuplicate].Value);
        }

        var category = await _categoryReadOnlyRepository.GetCategoryByIdAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new BadRequestException(_localizer["category_does_not_exist_or_was_deleted"].Value);
        }
        
        var parent = await GetAndValidateParentAsync(request.ParentId, cancellationToken);

        category = _mapper.Map(request, category);
        UpdateLevelAndPath(category, parent);
        
        await _categoryWriteOnlyRepository.UpdateCategoryAsync(category, cancellationToken);
        await _categoryWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken);
        
        return Unit.Value;
    }
    
    private async Task<Category?> GetAndValidateParentAsync(Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (parentId == null || parentId == Guid.Empty)
            return null;

        var parent = await _categoryReadOnlyRepository.GetCategoryByIdAsync(parentId.Value, cancellationToken);
        if (parent is null)
        {
            throw new BadRequestException(_localizer["category_parent_id_is_invalid"].Value);
        }

        return parent;
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