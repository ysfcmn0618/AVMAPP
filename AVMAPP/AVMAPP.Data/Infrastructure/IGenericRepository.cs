using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Infrastructure
{
    internal interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        public IQueryable<T> GetAll();
        Task<T> GetByIdAsync(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(string id);
        // Eager Loading için eklenen metod
        Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdIncludingAsync(string id, params Expression<Func<T, object>>[] includes);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
 