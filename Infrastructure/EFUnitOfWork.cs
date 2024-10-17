using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Abstraction;

namespace WebBlog.Infrastructure
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public object GetDbContext()
        {
            return _context;
        }

        public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync();
        }
    }
}
