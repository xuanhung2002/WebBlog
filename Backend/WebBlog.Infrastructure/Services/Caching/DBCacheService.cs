using WebBlog.Application.Interfaces.Caching;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Services.Caching
{
    public class DBCacheService : IDBCacheService
    {
        public async Task RefreshCache<T>(Guid? id = null)
        {
            throw new NotImplementedException();
        }  
    }
}
