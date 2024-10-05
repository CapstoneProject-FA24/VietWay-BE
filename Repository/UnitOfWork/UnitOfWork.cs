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
        private IGenericRepository<Account>? accountRepository;
        private IGenericRepository<Customer>? customerInfoRepository;
        private IGenericRepository<Image>? imageRepository;
        private IGenericRepository<Manager>? managerInfoRepository;
        private IGenericRepository<Province>? provinceRepository;
        private IGenericRepository<Tour>? tourRepository;
        private IGenericRepository<TourTemplate>? tourTemplateRepository;
        private IGenericRepository<Staff>? staffRepository;
        private IGenericRepository<TourCategory>? categoryRepository;
        private IGenericRepository<AttractionType>? attractionTypeRepository;
        private IGenericRepository<Attraction>? attractionRepository;
        private IGenericRepository<AttractionImage>? attractionImageRepository;
        private IGenericRepository<TourDuration>? durationRepository;
        private IGenericRepository<TourTemplateSchedule>? tourTemplateScheduleRepository;
        private IGenericRepository<AttractionSchedule>? attractionScheduleRepository;

        public IGenericRepository<TourTemplateSchedule> TourTemplateScheduleRepository
        {
            get
            {
                tourTemplateScheduleRepository ??= new GenericRepository<TourTemplateSchedule>(_dbContext);
                return tourTemplateScheduleRepository;
            }
        }
        public IGenericRepository<AttractionSchedule> AttractionScheduleRepository
        {
            get
            {
                attractionScheduleRepository ??= new GenericRepository<AttractionSchedule>(_dbContext);
                return attractionScheduleRepository;
            }
        }
        public IGenericRepository<AttractionImage> AttractionImageRepository
        {
            get
            {
                attractionImageRepository ??= new GenericRepository<AttractionImage>(_dbContext);
                return attractionImageRepository;
            }
        }
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

        public IGenericRepository<TourCategory> TourCategoryRepository
        {
            get
            {
                this.categoryRepository ??= new GenericRepository<TourCategory>(_dbContext);
                return this.categoryRepository;
            }
        }

        public IGenericRepository<AttractionType> AttractionTypeRepository
        {
            get
            {
                this.attractionTypeRepository ??= new GenericRepository<AttractionType>(_dbContext);
                return this.attractionTypeRepository;
            }
        }

        public IGenericRepository<TourDuration> TourDurationRepository
        {
            get
            {
                this.durationRepository ??= new GenericRepository<TourDuration>(_dbContext);
                return this.durationRepository;
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
