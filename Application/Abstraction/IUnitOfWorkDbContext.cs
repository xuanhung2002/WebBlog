using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Abstraction
{
    public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable
        where TContext : class // implement must be DbContext
    {
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
        TContext GetDbContext();
    }
}
