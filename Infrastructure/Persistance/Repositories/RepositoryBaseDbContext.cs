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

        public async Task<T> AddAsync<T>(T entity, bool clearTracker = false) where T : class
        {
            if (entity is EntityAuditBase baseEntity)
            {
                baseEntity.CreatedDate = DateTime.Now;
            }
            var res = await _context.AddAsync(entity);
            await SaveChangesAsync(clearTracker);
            return res.Entity;
        }

        public async Task<int> AddRangeAsync<T>(IEnumerable<T> entities, bool clearTracker = false) where T : class
        {
            foreach (var entity in entities)
            {
                if (entity is EntityAuditBase baseEntity)
                {
                    baseEntity.CreatedDate = DateTime.Now;
                }
            }
            await _context.Set<T>().AddRangeAsync(entities);
            var res = await SaveChangesAsync(clearTracker);
            return res;
        }

        public async Task<int> DeleteAsync<T>(T entity, bool clearTracker = false) where T : class
        {
            _context.Set<T>().Remove(entity);
            var res = await SaveChangesAsync(clearTracker);
            return res;

        }

        public async Task<int> DeleteRangeAsync<T>(IEnumerable<T> entities, bool clearTracker = false) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
            var res = await SaveChangesAsync(clearTracker);
            return res;
        }

        public async Task<T?> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FindForUpdateAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AsTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> predicate = default) where T : class
        {
            if (predicate == null)
            {
                return await _context.Set<T>().ToListAsync();
            }
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<R>> GetAsync<T, R>(Expression<Func<T, R>> selector, Expression<Func<T, bool>> predicate = default) where T : class
        {
            if (predicate == null)
            {
                return await _context.Set<T>().Select(selector).ToListAsync();
            }
            return await _context.Set<T>().Where(predicate).Select(selector).ToListAsync();
        }

        public IQueryable<T> GetSet<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            if (predicate == null)
            {
                return _context.Set<T>().AsNoTracking();
            }
            return _context.Set<T>().Where(predicate);
        }

        public IQueryable<T?> GetSetAsTracking<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            if (predicate == null)
            {
                return _context.Set<T>();
            }
            return _context.Set<T>().Where(predicate);
        }



        public async Task<int> SaveChangesAsync(bool clearTracker = false)
        {
            var res = await _context.SaveChangesAsync();
            if (clearTracker)
            {
                _context.ChangeTracker.Clear();
            }
            return res;
        }

        public async Task<T> UpdateAsync<T>(T entity, bool clearTracker = true) where T : class
        {
            if (entity is EntityAuditBase baseEntity)
            {
                baseEntity.ModifiedDate = DateTime.UtcNow;
            }
            var res = _context.Set<T>().Update(entity);
            await SaveChangesAsync(clearTracker);
            return res.Entity;
        }

        public async Task<int> UpdateRangeAsync<T>(IEnumerable<T> entities, bool clearTracker = false) where T : class
        {
            foreach (var entity in entities)
            {
                if (entity is EntityAuditBase baseEntity)
                {
                    baseEntity.ModifiedDate = DateTime.UtcNow;
                }
            }
            var res = await _context.SaveChangesAsync(clearTracker);
            if (clearTracker)
            {
                _context.ChangeTracker.Clear();
            }
            return res;
        }
    }
}
