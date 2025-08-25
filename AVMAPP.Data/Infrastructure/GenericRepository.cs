using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure.AVMDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AVMAPP.Data.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AVMAppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AVMAppDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> GetAll(bool asNoTracking = false)
        {
            return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(e => ((IGenericField)e).Id == id);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task<T?> GetSingleWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.SingleOrDefaultAsync(predicate);
        }
    }
}
