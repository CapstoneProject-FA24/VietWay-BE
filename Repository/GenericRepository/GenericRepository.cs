using Microsoft.EntityFrameworkCore;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.GenericRepository
{
    public class GenericRepository<T>(VietWayDbContext dbContext) : IGenericRepository<T> where T : class
    {
        public readonly VietWayDbContext _dbContext = dbContext;
        public readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet?.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
        public async Task SoftDeleteAsync(T entity)
        {
            if (entity is SoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbSet.Update(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return _dbContext.SaveChangesAsync();
        }
    }
}
