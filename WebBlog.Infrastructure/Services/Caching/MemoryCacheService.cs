using Microsoft.Extensions.Caching.Memory;
using WebBlog.Application.Interfaces.Caching;

namespace WebBlog.Infrastructure.Services.Caching
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }
            return default(T); 
        }

        public bool Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(10));

            _memoryCache.Set(key, value, options);
            return true;
        }


        public bool Remove(string key)
        {
            if(_memoryCache.TryGetValue(key, out object _))
            {
                _memoryCache.Remove(key);
                return true;
            }
            return false;
        }

        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public async Task<T> GetWithAdd<T>(string key, Func<Task<T>> addCacheFunc, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }
          
            value = await addCacheFunc();

            Set(key, value, expiration);

            return value; 
        }
    }
}
