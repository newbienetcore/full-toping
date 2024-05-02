﻿namespace Shared.Caching.Abstractions;

public interface ICacheService
{
    Task<bool> ExistsAsync(string key);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false) where T : class;
    Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false);
    Task<bool> DeleteAsync(string key);
    Task DeleteByPatternAsync(string pattern);
    Task<T> GetAsync<T>(string key) where T : class;
    Task<string> GetStringAsync(string key);
    Task<bool> ReplaceAsync(string key, object value);
}