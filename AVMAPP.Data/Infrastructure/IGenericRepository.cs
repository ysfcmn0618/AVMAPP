
using System.Linq.Expressions;


namespace AVMAPP.Data.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAll(bool asNoTracking = false);
        Task<T> GetByIdAsync(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);

        // Eager Loading için eklenen metodlar
        Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetSingleWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    }
}
 