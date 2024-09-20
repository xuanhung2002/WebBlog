using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Abstraction.Repositories;
using WebBlog.Domain.Abstraction;
using WebBlog.Domain.Abstraction.Entities;

namespace WebBlog.Infrastructure.Persistance.Repositories
{
    public class RepositoryBaseDbContext<TContext, T, TKey> : IRepositoryBaseDbContext<TContext, T, TKey>
        where TContext : DbContext
        where T : EntityAuditBase<TKey>

    {
        private readonly TContext _context;
        public RepositoryBaseDbContext(TContext context)
        {
            _context = context;
        }

        public IQueryable<T?> FindAll(params Expression<Func<T, object>>[] includeProperties)
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

        public IQueryable<T?> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
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

        public async Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(predicate, cancellationToken);
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Remove(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public async Task RemoveAsync(TKey id, CancellationToken cancellation = default)
        {
            var entity = await FindByIdAsync(id, cancellation);
            Remove(entity);
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
