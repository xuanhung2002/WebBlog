using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Abstraction;

namespace WebBlog.Infrastructure.UoWMultiContext
{
    public abstract class EFUnitOfWorkDbContext<TContext> : IUnitOfWorkDbContext<TContext>
        where TContext : DbContext
    {
        private readonly TContext _context;
        public EFUnitOfWorkDbContext(TContext context)
        {
            _context = context;
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public TContext GetDbContext()
        {
            return _context;
        }

        public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync();
        }
    }
}
