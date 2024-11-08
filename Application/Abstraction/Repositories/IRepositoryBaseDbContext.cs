using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Domain.Abstraction;

namespace WebBlog.Application.Abstraction.Repositories
{
    public interface IRepositoryBaseDbContext
    {
        Task<T?> FindByIdAsync<T>(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties) where T : EntityAuditBase;
        Task<T?> FindSingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default, params Expression<Func<T, object>>[] includeProperties) where T : class;
        IQueryable<T?> FindAll<T>(params Expression<Func<T, object>>[] includeProperties) where T : class;
        IQueryable<T?> FindAll<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task RemoveAsync<T>(Guid id, CancellationToken cancellation = default) where T : EntityAuditBase;
        void RemoveMultiple<T>(List<T> entities) where T : class;
    }
}
