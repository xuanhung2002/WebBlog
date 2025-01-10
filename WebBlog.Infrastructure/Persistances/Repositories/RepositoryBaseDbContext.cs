using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using WebBlog.Application.Abstraction;
using WebBlog.Application.Interfaces;
using WebBlog.Application.Interfaces.Caching;
using WebBlog.Domain.Constant;
using WebBlog.Domain.Entities;

namespace WebBlog.Infrastructure.Persistances
{
    public class RepositoryBaseDbContext<TContext> : IRepositoryBaseDbContext
        where TContext : DbContext

    {        
        private TContext context = null;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;
        protected TContext _context
        {
            get
            {
                if (context == null)
                {
                    lock (_serviceProvider)
                    {
                        if (context == null)
                        {
                            context = _serviceProvider.GetService<TContext>();
                        }
                    }
                }

                return context;
            }
        }
        public RepositoryBaseDbContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = _serviceProvider.GetService<ICurrentUserService>();
        }

        private void InitBaseEntity<T>(T entity) where T : class
        {
            if (entity is EntityAuditBase baseEntity)
            {
                DateTimeOffset createdDate = ((DateTimeOffset)(baseEntity.ModifiedDate = DateTimeOffset.UtcNow));
                baseEntity.CreatedDate = createdDate;
                Guid createdBy = (Guid)(baseEntity.ModifiedBy = _currentUserService.GetCurrentUser()?.Id ?? SystemConstants.SYSTEMACCOUNTID);
                baseEntity.CreatedBy = createdBy;
            }
        }

        public async Task<T> AddAsync<T>(T entity, bool clearTracker = false) where T : class
        {
            InitBaseEntity(entity);
            var res = await _context.AddAsync(entity);
            await SaveChangesAsync(clearTracker);
            return res.Entity;
        }

        public async Task<int> AddRangeAsync<T>(IEnumerable<T> entities, bool clearTracker = false) where T : class
        {
            foreach (var entity in entities)
            {
                InitBaseEntity(entity);
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
            var result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
            return result;
        }

        public async Task<T?> FindForUpdateAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AsTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AnyAsync(predicate);
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

        private void UpdateBaseEntity<T>(T entity) where T : class
        {
            if (entity is EntityAuditBase baseEntity)
            {
                baseEntity.ModifiedDate = DateTime.UtcNow;
                baseEntity.ModifiedBy = (Guid)(baseEntity.ModifiedBy = _currentUserService.GetCurrentUser()?.Id ?? SystemConstants.SYSTEMACCOUNTID);
            }
        }
        public async Task<T> UpdateAsync<T>(T entity, bool clearTracker = true) where T : class
        {
            if (entity is EntityAuditBase baseEntity)
            {
                UpdateBaseEntity(entity);
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
                    UpdateBaseEntity(entity);
                }
            }
            _context.Set<T>().UpdateRange(entities);
            var res = await _context.SaveChangesAsync(clearTracker);
            if (clearTracker)
            {
                _context.ChangeTracker.Clear();
            }
            return res;
        }

        //    public async Task<T> FindFromCacheAsync<T>(Expression<Func<T, bool>> predicate = null) where T : class
        //    {
        //        var cacheKey = GetCacheKey<T>(predicate);

        //        var cachedData = _cacheService.Get<T>(cacheKey);
        //        if (cachedData != null)
        //        {
        //            return cachedData; 
        //        }

        //        var result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

        //        if (result != null)
        //        {
        //            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        //        }

        //        return result;
        //    }

        //    public async Task<List<T>> GetFromCacheAsync<T>(Expression<Func<T, bool>> predicate = null) where T : class
        //    {
        //        var cacheKey = GetCacheKey<T>(predicate);

        //        var cachedData = _cacheService.Get<List<T>>(cacheKey);
        //        if (cachedData != null)
        //        {
        //            return cachedData;
        //        }

        //        var result = await _context.Set<T>().Where(predicate).ToListAsync();

        //        if (result.Any())
        //        {
        //            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10)); 
        //        }

        //        return result;
        //    }

        //    private string GetCacheKey<T>(Expression<Func<T, bool>> predicate)
        //    {
        //        var entityType = typeof(T).Name;
        //        var predicateString = predicate != null ? predicate.ToString() : "all";

        //        return $"{entityType}_{predicateString}";
        //    }
        //    private async Task RemoveCacheAsync(Type type)
        //    {
        //        var cacheKey = $"{type.Name}_";
        //        _cacheService.Remove(cacheKey);
        //    }
    }
}
