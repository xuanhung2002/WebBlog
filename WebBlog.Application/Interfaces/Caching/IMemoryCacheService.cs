using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Interfaces.Caching
{
    public interface IMemoryCacheService
    {
        T Get<T>(string key);
        bool Set<T>(string key, T value, TimeSpan? expiration = null);
        bool Exists(string key);
        Task<T> GetWithAdd<T>(string key, Func<Task<T>> addCacheFunc, TimeSpan? expiration = null);
        bool Remove(string key);
    }
}
