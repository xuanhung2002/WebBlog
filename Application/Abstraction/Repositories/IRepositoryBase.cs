using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Application.Abstraction.Repositories
{
    public interface IRepositoryBase<T, in TKey> where T : class
    {
        Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T?> FindAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task RemoveAsync(TKey id, CancellationToken cancellation = default);
        void RemoveMultiple(List<T> entities);
    }
}
