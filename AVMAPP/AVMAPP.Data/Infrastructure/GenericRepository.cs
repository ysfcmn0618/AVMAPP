using AVMAPP.Data.Infrastructure.AVMDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Infrastructure
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AVMAppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AVMAppDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public IQueryable<T> GetAllAsync()
        {
            return _dbContext.Set<T>();
        }
        public async Task<T> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                return entity;
            }
            throw new KeyNotFoundException($"Entity {typeof(T).Name} with ID {id} not found.");
        }

        public async Task<T> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
