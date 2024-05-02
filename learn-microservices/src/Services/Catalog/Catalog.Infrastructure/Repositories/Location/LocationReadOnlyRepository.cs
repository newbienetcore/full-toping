using AutoMapper;
using AutoMapper.QueryableExtensions;
using Caching;
using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace Catalog.Infrastructure.Repositories;

public class LocationReadOnlyRepository : ILocationReadOnlyRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISequenceCaching _caching;
    public LocationReadOnlyRepository(
        ApplicationDbContext context, 
        IMapper mapper,
        ISequenceCaching caching)
    {
        _context = context;
        _mapper = mapper;
        _caching = caching;
    }

    public async Task<LocationProvince?> GetProvinceByIdAsync(long provinceId, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationProvince)Activator.CreateInstance(typeof(LocationProvince))).GetTableName();
        string key = BaseCacheKeys.GetSystemRecordByIdKey(tableName, provinceId);
        
        var province = await _caching.GetAsync<LocationProvince>(key, cancellationToken: cancellationToken);
        if (province != null)
        {
            return province;
        }

        province = await _context.Provinces.FirstOrDefaultAsync(e => e.Id == provinceId, cancellationToken);
        if (province != null)
        {
            await _caching.SetAsync(key, province, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return province;
    }

    public async Task<LocationDistrict?> GetDistrictByIdAsync(long districtId, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationDistrict)Activator.CreateInstance(typeof(LocationDistrict))).GetTableName();
        string key = BaseCacheKeys.GetSystemRecordByIdKey(tableName, districtId);
        
        var district = await _caching.GetAsync<LocationDistrict>(key, cancellationToken: cancellationToken);
        if (district != null)
        {
            return district;
        }

        district = await _context.Districts.FirstOrDefaultAsync(e => e.Id == districtId, cancellationToken);
        if (district != null)
        {
            await _caching.SetAsync(key, district, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return district;
    }

    public async Task<LocationProvince?> GetProvinceByCodeAsync(string provinceCode, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationProvince)Activator.CreateInstance(typeof(LocationProvince))).GetTableName();
        string key = BaseCacheKeys.GetSystemRecordByIdKey(tableName, provinceCode);
        
        var province = await _caching.GetAsync<LocationProvince>(key, cancellationToken: cancellationToken);
        if (province != null)
        {
            return province;
        }

        province = await _context.Provinces.FirstOrDefaultAsync(e => e.Code == provinceCode, cancellationToken);
        if (province != null)
        {
            await _caching.SetAsync(key, province, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return province;
    }

    public async Task<LocationDistrict?> GetDistrictByCodeAsync(string districtCode, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationDistrict)Activator.CreateInstance(typeof(LocationDistrict))).GetTableName();
        string key = BaseCacheKeys.GetSystemRecordByIdKey(tableName, districtCode);
        
        var district = await _caching.GetAsync<LocationDistrict>(key, cancellationToken: cancellationToken);
        if (district != null)
        {
            return district;
        }

        district = await _context.Districts.FirstOrDefaultAsync(e => e.Code == districtCode, cancellationToken);
        if (district != null)
        {
            await _caching.SetAsync(key, district, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return district;
    }

    public async Task<IList<LocationProvinceDto>> GetAllProvincesAsync(CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationProvince)Activator.CreateInstance(typeof(LocationProvince))).GetTableName();
        string key = BaseCacheKeys.GetSystemFullRecordsKey(tableName);

        var provinces = await _caching.GetAsync<IList<LocationProvinceDto>>(key, cancellationToken: cancellationToken);
        if (provinces != null)
        {
            return provinces;
        }

        provinces = await _context.Provinces
            .ProjectTo<LocationProvinceDto>(_mapper.ConfigurationProvider, cancellationToken)
            .ToListAsync(cancellationToken);

        if (provinces != null)
        {
            await _caching.SetAsync(key, provinces, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return provinces;

    }
    
    public async Task<IList<LocationDistrictDto>> GetAllDistrictsByProvinceIdAsync(long provinceId, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationDistrict)Activator.CreateInstance(typeof(LocationDistrict))).GetTableName();
        string key = BaseCacheKeys.GetSystemFullRecordsKey(tableName);

        var districts = await _caching.GetAsync<IList<LocationDistrictDto>>(key, cancellationToken: cancellationToken);
        if (districts != null)
        {
            return districts;
        }

        districts = await _context.Districts
            .Where(e => e.ProvinceId == provinceId)
            .ProjectTo<LocationDistrictDto>(_mapper.ConfigurationProvider, cancellationToken)
            .ToListAsync(cancellationToken);

        if (districts != null)
        {
            await _caching.SetAsync(key, districts, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return districts;
    }

    public async Task<IList<LocationWardDto>> GetAllWardsByDistrictIdAsync(long districtId, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationWard)Activator.CreateInstance(typeof(LocationWard))).GetTableName();
        string key = BaseCacheKeys.GetSystemFullRecordsKey(tableName);

        var wards = await _caching.GetAsync<IList<LocationWardDto>>(key, cancellationToken: cancellationToken);
        if (wards != null)
        {
            return wards;
        }

        wards = await _context.Wards
            .Where(e => e.DistrictId == districtId)
            .ProjectTo<LocationWardDto>(_mapper.ConfigurationProvider, cancellationToken)
            .ToListAsync(cancellationToken);

        if (wards != null)
        {
            await _caching.SetAsync(key, wards, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return wards;
    }

    public async Task<IList<LocationDistrictDto>> GetAllDistrictsByProvinceCodeAsync(string provinceCode, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationDistrict)Activator.CreateInstance(typeof(LocationDistrict))).GetTableName();
        string key = BaseCacheKeys.GetSystemFullRecordsKey(tableName);

        var districts = await _caching.GetAsync<IList<LocationDistrictDto>>(key, cancellationToken: cancellationToken);
        if (districts != null)
        {
            return districts;
        }

        districts = await _context.Districts
            .Where(e => e.Province.Code == provinceCode)
            .ProjectTo<LocationDistrictDto>(_mapper.ConfigurationProvider, cancellationToken)
            .ToListAsync(cancellationToken);

        if (districts != null)
        {
            await _caching.SetAsync(key, districts, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return districts;
    }

    public async Task<IList<LocationWardDto>> GetAllWardsByDistrictCodeAsync(string districtCode, CancellationToken cancellationToken = default)
    {
        var tableName = ((LocationWard)Activator.CreateInstance(typeof(LocationWard))).GetTableName();
        string key = BaseCacheKeys.GetSystemFullRecordsKey(tableName);

        var wards = await _caching.GetAsync<IList<LocationWardDto>>(key, cancellationToken: cancellationToken);
        if (wards != null)
        {
            return wards;
        }

        wards = await _context.Wards
            .Where(e => e.District.Code == districtCode)
            .ProjectTo<LocationWardDto>(_mapper.ConfigurationProvider, cancellationToken)
            .ToListAsync(cancellationToken);

        if (wards != null)
        {
            await _caching.SetAsync(key, wards, TimeSpan.FromDays(30), onlyUseType: CachingType.Redis, cancellationToken: cancellationToken);
        }

        return wards;
    }
}