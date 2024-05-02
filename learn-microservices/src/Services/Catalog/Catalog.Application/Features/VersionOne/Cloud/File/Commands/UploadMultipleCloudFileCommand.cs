using Catalog.Application.DTOs;
using Catalog.Application.Properties;
using Catalog.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Application.Features.VersionOne;

public class UploadMultipleCloudFileCommand : BaseInsertCommand<IList<AssetDto>>
{
    public List<IFormFile> Files { get; }

    public UploadMultipleCloudFileCommand(List<IFormFile> files)
    {
        Files = files;
    }
}

public class UploadMultipleCloudFileCommandValidator : AbstractValidator<UploadMultipleCloudFileCommand>
{
    public UploadMultipleCloudFileCommandValidator(IStringLocalizer<Resources> localizer, IFileService fileService)
    {
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage(localizer["file_is_required"].Value);

        RuleForEach(x => x.Files)
            .MustAsync(async (file, cancellationToken) =>
            {
                try
                {
                    await fileService.CheckAcceptFileExtensionAndThrow(file);
                    return true;
                }
                catch (BadRequestException)
                {
                    return false;
                }
            })
            .WithMessage(localizer["file_extension_is_not_valid"].Value);

        RuleForEach(x => x.Files)
            .Must(file => file.Length < 4 * 1024 * 1024)
            .WithMessage(localizer["file_must_be_smaller_than_4_MB"].Value)
            .When(x => x.Files != null && x.Files.Any());
    }
}