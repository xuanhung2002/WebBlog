using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Interfaces.Caching
{
    public interface IDBCacheService
    {
        Task RefreshCache<T>(Guid? id = null);

    }
}
