using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<Attraction> AttractionRepository { get; }
        public IGenericRepository<Customer> CustomerInfoRepository { get; }
        public IGenericRepository<Image> ImageRepository { get; }
        public IGenericRepository<Manager> ManagerInfoRepository { get; }
        public IGenericRepository<Province> ProvinceRepository { get; }
        public IGenericRepository<TourTemplate> TourTemplateRepository { get; }
        public IGenericRepository<Tour> TourRepository { get; }
        public IGenericRepository<Staff> StaffRepository { get; }
        public IGenericRepository<TourCategory> TourCategoryRepository { get; }
        public IGenericRepository<AttractionType> AttractionTypeRepository { get; }
        public IGenericRepository<AttractionImage> AttractionImageRepository { get; }
        public IGenericRepository <TourDuration> TourDurationRepository { get; }
        void Save();
    }
}
