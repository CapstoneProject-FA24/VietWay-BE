using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public void Create(T entity);

        public void Delete(T entity);

        public void Update(T entity);

    }
}
