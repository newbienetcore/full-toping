using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Polly.Retry;
using Shared.Caching.Abstractions;
using StackExchange.Redis;

namespace Shared.Caching.Redis;

public class CacheService : ICacheService
{
    private readonly IDatabase _cache;
    private readonly string _instance;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IServer _server;

    public CacheService(string connectionString, string prefix, int database = 0,
        AsyncRetryPolicy? asyncRetryPolicy = null)
    {
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _cache = connection.GetDatabase(database);
        _server = connection.GetServer(connectionString);
        _instance = prefix;
        _retryPolicy = asyncRetryPolicy;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        return await RunWithPolicyAsync(async () => await _cache.KeyExistsAsync(GetKeyForRedis(key)));
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, bool keepTtl = false) where T : class
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (value is null)
            throw new ArgumentNullException(nameof(value));

        var valueString = JsonConvert.SerializeObject(value);
        return await RunWithPolicyAsync(async () => await SetStringAsync(key, valueString, expiry, keepTtl));
    }

    public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null, bool keepTtl = false)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        return await RunWithPolicyAsync(async () =>
            await _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(value), expiry, keepTtl));
    }

    public async Task<bool> DeleteAsync(string key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        return await RunWithPolicyAsync(async () => await _cache.KeyDeleteAsync(GetKeyForRedis(key)));
    }

    public async Task DeleteByPatternAsync(string pattern)
    {
        if (_server != null && !string.IsNullOrEmpty(pattern))
        {
            var keys = _server.Keys(pattern: GetKeyForRedis(pattern)).ToArray();

            await RunWithPolicyAsync(async () =>
            {
                await _cache.KeyDeleteAsync(keys);
            });
        }
    }

    public async Task<T> GetAsync<T>(string key) where T : class
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return await RunWithPolicyAsync(async () =>
        {
            var value = await GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(value))
                return default;
            
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    public async Task<string> GetStringAsync(string key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        

        return await RunWithPolicyAsync(async () =>
        {
            var value = await _cache.StringGetAsync(GetKeyForRedis(key));

            if (!value.HasValue)
                return default;
            
            return Encoding.UTF8.GetString(value);
        });
    }

    public async Task<bool> ReplaceAsync(string key, object value)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        

        return await RunWithPolicyAsync(async () =>
        {
            if (await ExistsAsync(key) && !await DeleteAsync(key))
                return false;
            
            return await SetAsync(key, value);
        });
    }

    #region [PRIVATE METHODS]

    private async Task<T> RunWithPolicyAsync<T>(Func<Task<T>> action)
    {
        if (_retryPolicy is null)
            return await action();

        return await _retryPolicy
            .ExecuteAsync(async () => await action())
            .ConfigureAwait(false);
    }

    private async Task RunWithPolicyAsync(Func<Task> action)
    {
        if (_retryPolicy is null)
        {
            await action();
            return;
        }

        await _retryPolicy
            .ExecuteAsync(async () =>
            {
                await action();
            }).ConfigureAwait(false);
    }

    private string GetKeyForRedis(string key) => $"{_instance}{key}";

    #endregion
}