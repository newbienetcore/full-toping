using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Providers;
using SharedKernel.Runtime.Exceptions;
using Enum = System.Enum;

namespace Catalog.Application.Features.VersionOne;

public class UploadMultipleCloudFileCommandHandler : BaseCommandHandler, IRequestHandler< UploadMultipleCloudFileCommand, IList<AssetDto>>
{
    private readonly IFirebaseStorageService _firebaseStorageService;
    private readonly IFileService _fileService;
    private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
    private readonly IAssetWriteOnlyRepository _assetWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    public UploadMultipleCloudFileCommandHandler(
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

    public async Task<IList<AssetDto>> Handle(UploadMultipleCloudFileCommand request, CancellationToken cancellationToken)
    {
        // await _fileService.CheckAcceptFileExtensionAndThrow(request.File);
        
        // Đẩy files lên firebase
        var uploadRequests = request.Files.Select(file => new UploadRequest
        {
            FileName = file.FileName,
            FileExtension = Path.GetExtension(file.FileName).ToLower(),
            Size = file.Length,
            Stream = file.OpenReadStream(),
        }).ToList();

        var uploadResponses = await _firebaseStorageService.UploadAsync(uploadRequests, cancellationToken);
        var successFiles = uploadResponses.Where(r => r.Success).ToList();

        // Đẩy files failed
        if (!successFiles.Any())
        {
            throw new CatchableException(_localizer["upload_image_failed"].Value);
        }

        var files = successFiles.Select(f => new Asset()
        {
            FileName = f.CurrentFileName,
            OriginalFileName = f.OriginalFileName,
            Description = String.Empty,
            FileExtension = f.FileExtension,
            Type = SharedKernel.Application.Enum.FileType.Image,
            Size = f.Size
        }).ToList();

        var fileResults = await _assetWriteOnlyRepository.InsertAsync(files, cancellationToken);
        await _assetWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

        var assetDtos = _mapper.Map<IList<AssetDto>>(fileResults).Select(assetDto =>
            {
                assetDto.Url = _fileService.GetFileUrl(assetDto.FileName);
                return assetDto;
            })
            .ToList();
        
        return assetDtos;
    }

}