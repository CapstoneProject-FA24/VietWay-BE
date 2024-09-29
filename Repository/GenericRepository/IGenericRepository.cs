using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task Create(T entity);

        public Task Delete(T entity);

        public Task Update(T entity);
        public IQueryable<T> Query();
    }
}
