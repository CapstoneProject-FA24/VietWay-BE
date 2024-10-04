using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VietWayDbContext _dbContext = new();
        private IGenericRepository<Account> accountRepository;
        private IGenericRepository<Customer> customerInfoRepository;
        private IGenericRepository<Image> imageRepository;
        private IGenericRepository<Manager> managerInfoRepository;
        private IGenericRepository<Province> provinceRepository;
        private IGenericRepository<Tour> tourRepository;
        private IGenericRepository<TourTemplate> tourTemplateRepository;
        private IGenericRepository<Staff> staffRepository;
        private IGenericRepository<Attraction> attractionRepository;

        public IGenericRepository<Attraction> AttractionRepository
        {
            get
            {
                attractionRepository ??= new GenericRepository<Attraction>(_dbContext);
                return attractionRepository;
            }
        }

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                accountRepository ??= new GenericRepository<Account>(_dbContext);
                return accountRepository;
            }
        }
        public IGenericRepository<Customer> CustomerInfoRepository
        {
            get
            {
                customerInfoRepository ??= new GenericRepository<Customer>(_dbContext);
                return customerInfoRepository;
            }
        }
        public IGenericRepository<Image> ImageRepository
        {
            get
            {
                imageRepository ??= new GenericRepository<Image>(_dbContext);
                return imageRepository;
            }
        }
        public IGenericRepository<Manager> ManagerInfoRepository
        {
            get
            {
                managerInfoRepository ??= new GenericRepository<Manager>(_dbContext);
                return managerInfoRepository;
            }
        }
        public IGenericRepository<Province> ProvinceRepository
        {
            get
            {
                provinceRepository ??= new GenericRepository<Province>(_dbContext);
                return provinceRepository;
            }
        }

        public IGenericRepository<TourTemplate> TourTemplateRepository
        {
            get
            {
                this.tourTemplateRepository ??= new GenericRepository<TourTemplate>(_dbContext);
                return this.tourTemplateRepository;
            }
        }

        public IGenericRepository<Tour> TourRepository
        {
            get
            {
                this.tourRepository ??= new GenericRepository<Tour>(_dbContext);
                return this.tourRepository;
            }
        }

        public IGenericRepository<Staff> StaffRepository
        {
            get
            {
                this.staffRepository ??= new GenericRepository<Staff>(_dbContext);
                return this.staffRepository;
            }
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
