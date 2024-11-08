using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebBlog.Application.Abstraction.Repositories;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Infrastructure.Persistance.Repositories
{
    public class RepositoryBaseDbContext<TContext> : IRepositoryBaseDbContext
        where TContext : DbContext

    {
        private readonly TContext _context;
        public RepositoryBaseDbContext(TContext context)
        {
            _context = context;
        }

        public IQueryable<T?> FindAll<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T?> items = _context.Set<T>().AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T?> FindAll<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T?> items = _context.Set<T>().AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public async Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties) where T : EntityAuditBase
        {
            
            return await FindAll(includeProperties).SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<T?> FindSingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(predicate, cancellationToken);
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }
        public async Task RemoveAsync<T>(Guid id, CancellationToken cancellation = default) where T : EntityAuditBase
        {
            var entity = await FindByIdAsync<T>(id, cancellation);
            Remove(entity);
        }

        public void RemoveMultiple<T>(List<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
