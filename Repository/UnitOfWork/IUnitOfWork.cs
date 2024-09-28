using Repository.ModelEntity;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<CustomerInfo> CustomerInfoRepository { get; }
        public IGenericRepository<Image> ImageRepository { get; }
        public IGenericRepository<ManagerInfo> ManagerInfoRepository { get; }
        public IGenericRepository<Province> ProvinceRepository { get; }
        void Save();
    }
}
