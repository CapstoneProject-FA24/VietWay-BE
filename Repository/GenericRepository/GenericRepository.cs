using Microsoft.EntityFrameworkCore;
using Repository.DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private VietWayDbContext _dbContext;
        private DbSet<T> _dbSet = null;
        public GenericRepository(VietWayDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet?.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        protected IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
