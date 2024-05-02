using System.Collections.Concurrent;
using System.Reflection;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Serilog;
using SharedKernel.Libraries;
using SharedKernel.Log;
using Enum = SharedKernel.Application.Enum;

namespace Catalog.Infrastructure.Persistence;

public class ApplicationDbContextSeed
{
    private readonly IServiceProvider _provider;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public ApplicationDbContextSeed(ApplicationDbContext context,
        IHttpContextAccessor accessor,
        IServiceProvider provider
    )
    {
        _provider = provider;
        _context = context;
        _accessor = accessor;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsMySql())
                await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Logging.Error("An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.CommitAsync();
        }
        catch (Exception e)
        {
            // Logging.Error("An error occurred while seeding the database;");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Weights.Any())
        {
            var weightCategories = new List<Weight>
            {
                new Weight { Code = "G", Description = "Gam" },
                new Weight { Code = "KG", Description = "Kilogram" },
            };

            _context.Weights.AddRange(weightCategories);
            await _context.SaveChangesAsync();
        }

        if (!_context.Provinces.Any())
        {
            await ReadAndSeedLocationsAsync();
        }

        await AssetSeed.SeedAssetAsync(_context);
        
        await CategorySeed.SeedCategoryAsync(_context);
        
        await SupplierSeed.SeedSupplierAsync(_context);
        
    }
    
    private async Task ReadAndSeedLocationsAsync()
    {
        string relativePath = "Excels/province-district-ward.xlsx";

        var dirPath = Assembly.GetExecutingAssembly().Location;
        string fullPath = Path.Combine(Path.GetDirectoryName(dirPath), relativePath);
        if (!File.Exists(fullPath))
        {
            return;
        }

        FileInfo fileInfo = new FileInfo(fullPath);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(fileInfo);
        await package.LoadAsync(fileInfo);
        var worksheet = package.Workbook.Worksheets[0];

        var row = 2;
        LocationProvince currentProvince = null;
        LocationDistrict currentDistrict = null;
        while (true)
        {
            var provinceName = worksheet.Cells[row, 1].Text;
            var provinceCode = worksheet.Cells[row, 2].Text;

            var districtName = worksheet.Cells[row, 3].Text;
            var districtCode = worksheet.Cells[row, 4].Text;

            var wardName = worksheet.Cells[row, 5].Text;
            var wardCode = worksheet.Cells[row, 6].Text;


            if (currentProvince != null && currentProvince.Code != provinceCode)
            {
                await _context.Provinces.AddAsync(currentProvince);
                await _context.SaveChangesAsync();

                currentProvince = null;
                currentDistrict = null;
            }

            if (currentProvince == null || currentProvince.Code != provinceCode)
            {
                currentProvince = new LocationProvince
                {
                    Code = provinceCode,
                    Name = provinceName,
                    Slug = provinceName.ToUnsignString(),
                    Type = LocationType.Province,
                    Districts = new List<LocationDistrict>()
                };
            }

            if (currentDistrict != null && currentDistrict.Code != districtCode)
            {
                currentProvince.Districts.Add(currentDistrict);
                currentDistrict = null;
            }

            if (currentDistrict == null || currentDistrict.Code != districtCode)
            {
                currentDistrict = new LocationDistrict
                {
                    Code = districtCode,
                    Name = districtName,
                    Slug = districtName.ToUnsignString(),
                    Type = LocationType.District,
                    Province = currentProvince,
                    Wards = new List<LocationWard>()
                };
            }

            currentDistrict.Wards.Add(new LocationWard
            {
                Code = wardCode,
                Name = wardName,
                Slug = wardName.ToUnsignString(),
                Type = LocationType.Ward,
                District = currentDistrict
            });


            if (row == worksheet.Dimension.Rows) break; // Nếu đã đọc hết dữ liệu
            row++;
        }
    }
}