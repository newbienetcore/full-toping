using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Enum = SharedKernel.Application.Enum;

namespace Catalog.Infrastructure.Persistence;

public static class AssetSeed
{
    public static async Task SeedAssetAsync(ApplicationDbContext _context)
    {
        if (await _context.Assets.AnyAsync()) return;

        var assets = new List<Asset>()
        {
            new Asset
            {
                FileName = "images/catalog/f4f59825-0800-4357-a7aa-ab235ee3375e.png",
                OriginalFileName = "1.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 46510,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/efc0ae67-28b2-47e0-ac02-c91ce79f47d3.png",
                OriginalFileName = "2.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 26306,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/54a430ed-fbb0-4761-b246-fd20fc9e1768.png",
                OriginalFileName = "3.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 55967,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/fbf1cf79-e9c3-433d-aafb-7aeb988d5fbc.png",
                OriginalFileName = "4.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 54327,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/3d70a09d-1eb5-43f1-a69f-14ab39c73092.png",
                OriginalFileName = "5.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 57662,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/7d739402-639d-4011-b7e3-f22a227ad37a.png",
                OriginalFileName = "6.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 44240,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/2a10b1d9-7953-44bf-a5b2-5c6420ea110b.png",
                OriginalFileName = "7.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 60560,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/4e6c5e69-4b1b-4d72-8b66-a7001d629b06.png",
                OriginalFileName = "8.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 56619,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/9415ae3a-cf33-467a-82ac-fee231eaae07.png",
                OriginalFileName = "9.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 76041,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/d8efd53f-1d5e-4552-9b0c-623de671825f.png",
                OriginalFileName = "10.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 43492,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/438e5423-993c-434c-b5f4-fdd6180bec21.png",
                OriginalFileName = "11.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 32053,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/4854f6e1-b735-4f6f-8937-faecd9478d8e.png",
                OriginalFileName = "12.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 40818,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/7f5fe912-8630-4751-b48a-c9fbfd1f9fae.png",
                OriginalFileName = "13.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 50974,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/f2b23b52-b232-47f5-bbbf-da7d83e1ce1a.png",
                OriginalFileName = "14.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 47584,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/60d0a10b-cae6-49a0-8de7-58a7c957d184.png",
                OriginalFileName = "15.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 34217,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/6f0d11ba-11c1-4704-8039-d8a189694830.png",
                OriginalFileName = "16.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 60610,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/e3de60fa-d5dc-432f-925a-0fe3f168957f.png",
                OriginalFileName = "17.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 24576,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/2924460f-bdb9-468f-be95-19077ce5bfe2.png",
                OriginalFileName = "18.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 54016,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/e36924a9-a83e-41c0-ade0-a85fb17e4151.png",
                OriginalFileName = "19.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 87716,
                Type = Enum.FileType.Image
            },
            new Asset
            {
                FileName = "images/catalog/eb102843-df32-44d9-ada7-fe5352ce4b32.png",
                OriginalFileName = "20.png",
                Description = String.Empty,
                FileExtension = ".png",
                Size = 30214,
                Type = Enum.FileType.Image
            }
        };

        await _context.Assets.AddRangeAsync(assets);
        await _context.SaveChangesAsync();
    }
}