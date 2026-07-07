using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Interfaces;
using StudentProj.Data; // Assuming this is where StudentDbcontext is based on user snippet
using System.Linq.Expressions;

namespace StudentProj.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly StudentDbcontext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(StudentDbcontext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (useNoTracking)
                return await query.AsNoTracking().Where(filter).ToListAsync();
            else
                return await query.Where(filter).ToListAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (useNoTracking)
                return await query.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await query.Where(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
