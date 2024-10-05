using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.GenericRepository
{
    public class GenericRepository<T>(VietWayDbContext dbContext) : IGenericRepository<T> where T : class
    {
        private readonly VietWayDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task Create(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet?.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task SoftDelete(T entity)
        {
            if (entity is SoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbSet.Update(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
        public Task DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChangesAsync();
        }
    }
}
