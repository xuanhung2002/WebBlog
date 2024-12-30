using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WebBlog.Application.ExternalServices;

namespace WebBlog.Infrastructure.ExternalServices
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            var cachedData = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
                return default;

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serializedData = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            
            await _distributedCache.SetStringAsync(key, serializedData, options);
        }
        public async Task<string?> GetStringAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _distributedCache.SetStringAsync(key, value, options);
        }
        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }


    }
}
