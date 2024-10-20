namespace VietWay.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task CreateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task DeleteRangeAsync(IEnumerable<T> entities);
        public Task SoftDeleteAsync(T entity);
        public Task UpdateAsync(T entity);
        public IQueryable<T> Query();
    }
}
