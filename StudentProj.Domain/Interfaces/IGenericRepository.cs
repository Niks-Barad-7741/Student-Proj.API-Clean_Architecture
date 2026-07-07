using System.Linq.Expressions;

namespace StudentProj.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false, params Expression<Func<T, object>>[] includes);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
