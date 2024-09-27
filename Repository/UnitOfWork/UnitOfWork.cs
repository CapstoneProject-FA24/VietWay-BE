using Microsoft.EntityFrameworkCore;
using Repository.DataAccessObject;
using Repository.ModelEntity;
using Repository.Repository;
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private VietWayDbContext _dbContext = new();
        private IGenericRepository<Account> accountRepository;
        private IGenericRepository<CustomerInfo> customerInfoRepository;
        private IGenericRepository<Image> imageRepository;
        private IGenericRepository<ManagerInfo> managerInfoRepository;
        private IGenericRepository<Province> provinceRepository;

        public UnitOfWork(VietWayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                if(this.accountRepository == null)
                {
                    this.accountRepository = new  GenericRepository<Account>(_dbContext);
                }
                return this.accountRepository;
            }
        }

        public IGenericRepository<CustomerInfo> CustomerInfoRepository
        {
            get
            {
                if (this.customerInfoRepository == null)
                {
                    this.customerInfoRepository = new GenericRepository<CustomerInfo>(_dbContext);
                }
                return this.customerInfoRepository;
            }
        }
        public IGenericRepository<Image> ImageRepository
        {
            get
            {
                if( this.imageRepository == null)
                {
                    this.imageRepository = new GenericRepository<Image>(_dbContext);
                }
                return this.imageRepository;
            }
        }

        public IGenericRepository<ManagerInfo> ManagerInfoRepository
        {
            get
            {
                if(this.managerInfoRepository == null)
                {
                    this.managerInfoRepository = new GenericRepository<ManagerInfo>(_dbContext);
                }
                return this.managerInfoRepository;
            }
        }
        public IGenericRepository<Province> ProvinceRepository
        {
            get
            {
                if(this.provinceRepository == null)
                {
                    this.provinceRepository = new GenericRepository<Province>(_dbContext);
                }
                return this.provinceRepository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
