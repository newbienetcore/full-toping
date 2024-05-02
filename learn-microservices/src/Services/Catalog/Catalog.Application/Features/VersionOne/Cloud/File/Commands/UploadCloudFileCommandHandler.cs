using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Core;
using SharedKernel.Providers;
using SharedKernel.Runtime.Exceptions;
using Enum = SharedKernel.Application.Enum;

namespace Catalog.Application.Features.VersionOne;

public class UploadCloudFileCommandHandler : BaseCommandHandler, IRequestHandler<UploadCloudFileCommand, AssetDto>
{
    
    private readonly IFirebaseStorageService _firebaseStorageService;
    private readonly IFileService _fileService;
    private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
    private readonly IAssetWriteOnlyRepository _assetWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    public UploadCloudFileCommandHandler(
        IServiceProvider provider, 
        IFirebaseStorageService firebaseStorageService, 
        IFileService fileService,
        IAssetReadOnlyRepository assetReadOnlyRepository, 
        IAssetWriteOnlyRepository assetWriteOnlyRepository, 
        IStringLocalizer<Resources> localizer, 
        IMapper mapper
        ) : base(provider)
    {
        _firebaseStorageService = firebaseStorageService;
        _fileService = fileService;
        _assetReadOnlyRepository = assetReadOnlyRepository;
        _assetWriteOnlyRepository = assetWriteOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<AssetDto> Handle(UploadCloudFileCommand request, CancellationToken cancellationToken)
    {
        // await _fileService.CheckAcceptFileExtensionAndThrow(request.File);
        
        // Đẩy files lên firebase
        var uploadRequest =  new UploadRequest
        {
            FileName = request.File.FileName,
            FileExtension = Path.GetExtension(request.File.FileName).ToLower(),
            Size = request.File.Length,
            Stream = request.File.OpenReadStream(),
        };

        var response = await _firebaseStorageService.UploadAsync(uploadRequest, cancellationToken);
        
        // Đẩy files failed
        if (!response.Success)
        {
            throw new CatchableException(_localizer["upload_image_failed"].Value);
        }

        var files = new Asset()
        {
            FileName = response.CurrentFileName,
            OriginalFileName = response.OriginalFileName,
            Description = string.Empty,
            FileExtension = response.FileExtension,
            Type = Enum.FileType.Image,
            Size = response.Size
        };

        var fileResult = await _assetWriteOnlyRepository.InsertAsync(files, cancellationToken);
        await _assetWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

        var assetDto = _mapper.Map<AssetDto>(fileResult);
        assetDto.Url = _fileService.GetFileUrl(assetDto.FileName);

        return assetDto;
    }
    
}