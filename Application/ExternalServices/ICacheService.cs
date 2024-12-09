using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.ExternalServices
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<string?> GetStringAsync(string key);
        Task SetStringAsync(string key, string value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
