using System.Linq.Expressions;

namespace StudentProj.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
