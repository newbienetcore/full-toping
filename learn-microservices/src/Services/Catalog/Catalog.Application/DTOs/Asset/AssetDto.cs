using Catalog.Application.Mappings;
using Catalog.Domain.Entities;
using SharedKernel.Libraries;
using FileType = SharedKernel.Application.Enum.FileType;

namespace Catalog.Application.DTOs;

public class AssetDto : IMapFrom<Asset>
{
    public Guid Id { get; set; }
    
    public string FileName { get; set; }

    public string OriginalFileName { get; set; }
    public string? Description { get; set; }
    
    public string FileExtension { get; set; }
    
    public string Url { get; set; } 
    public long Size { get; set; }
    
    public FileType FileType => FileHelper.IsImage(FileName) ? FileType.Image : FileHelper.IsVideo(FileName) ? FileType.Video : FileType.Other;
    
    
}