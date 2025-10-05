using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.DAL;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UserModule.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
         where T : class
    {
        private readonly IDataContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(IDataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        
        public virtual IQueryable<T> AsTableNoTracking => _dbSet.AsNoTracking();
        public virtual IQueryable<T> AsTrackingEntity => _dbSet;

        public virtual EntityState Add(T entity)
        {
            return _dbSet.Add(entity).State;
        }
        
        public virtual async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return (await _dbSet.AddAsync(entity));
        }

        public T Get<TKey>(TKey id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }
        
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string include)
        {
            return FindBy(predicate).Include(include);
        }
        
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(int page, int pageCount)
        {
            var pageSize = (page - 1) * pageCount;

            return _dbSet.Skip(pageSize).Take(pageCount);
        }
        
        public IQueryable<T> GetAll(string include)
        {
            return _dbSet.Include(include);
        }
        
        public IQueryable<T> RawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters);
        }
        
        public IQueryable<T> GetAll(string include, string include2)
        {
            return _dbSet.Include(include).Include(include2);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual EntityState SoftDelete(T entity)
        {
            entity.GetType().GetProperty("IsSoftDeleted")?.SetValue(entity, true);
            return _dbSet.Update(entity).State;
        }

        public virtual EntityState HardDelete(T entity)
        {
            return _dbSet.Remove(entity).State;
        }
        public virtual EntityState Update(T entity)
        {
            return _dbSet.Update(entity).State;
        }
    }
}