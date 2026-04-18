using Microsoft.Extensions.Caching.Memory;
using MiniLedger.Api.Services.Interfaces;

namespace MiniLedger.Api.Services.Implementations;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
    {
        if (_memoryCache.TryGetValue(key, out T? cachedValue))
        {
            return cachedValue;
        }

        var value = await factory();

        _memoryCache.Set(key, value, expiration);

        return value;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}