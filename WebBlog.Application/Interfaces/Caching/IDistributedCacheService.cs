namespace WebBlog.Application.Interfaces.Caching
{
    public interface IDistributedCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<string?> GetStringAsync(string key);
        Task SetStringAsync(string key, string value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
