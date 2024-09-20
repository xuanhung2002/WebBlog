using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
        object GetDbContext();
    }
}
