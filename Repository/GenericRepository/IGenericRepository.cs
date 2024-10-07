namespace VietWay.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task Create(T entity);

        public Task Delete(T entity);
        public Task DeleteRange(IEnumerable<T> entities);
        public Task SoftDelete(T entity);
        public Task Update(T entity);
        public IQueryable<T> Query();
    }
}
