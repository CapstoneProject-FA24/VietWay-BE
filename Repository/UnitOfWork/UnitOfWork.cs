using Microsoft.EntityFrameworkCore.Storage;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VietWayDbContext _dbContext = new();

        public IDbContextTransaction? transaction;

        private IGenericRepository<Account>? accountRepository;
        private IGenericRepository<Admin>? adminRepository;
        private IGenericRepository<Attraction>? attractionRepository;
        private IGenericRepository<AttractionCategory>? attractionCategoryRepository;
        private IGenericRepository<Booking>? bookingRepository;
        private IGenericRepository<BookingPayment>? bookingPaymentRepository;
        private IGenericRepository<Customer>? customerRepository;
        private IGenericRepository<EntityHistory>? entityHistoryRepository;
        private IGenericRepository<EntityStatusHistory>? entityStatusHistoryRepository;
        private IGenericRepository<Event>? eventRepository;
        private IGenericRepository<EventCategory>? eventCategoryRepository;
        private IGenericRepository<TourReview>? feedbackRepository;
        private IGenericRepository<Manager>? managerRepository;
        private IGenericRepository<Post>? postRepository;
        private IGenericRepository<PostCategory>? postCategoryRepository;
        private IGenericRepository<Province>? provinceRepository;
        private IGenericRepository<Staff>? staffRepository;
        private IGenericRepository<Tour>? tourRepository;
        private IGenericRepository<TourCategory>? tourCategoryRepository;
        private IGenericRepository<TourDuration>? tourDurationRepository;
        private IGenericRepository<TourTemplate>? tourTemplateRepository;

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                accountRepository ??= new GenericRepository<Account>(_dbContext);
                return accountRepository;
            }
        }

        public IGenericRepository<Admin> AdminRepository
        {
            get
            {
                adminRepository ??= new GenericRepository<Admin>(_dbContext);
                return adminRepository;
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

        public IGenericRepository<AttractionCategory> AttractionCategoryRepository
        {
            get
            {
                attractionCategoryRepository ??= new GenericRepository<AttractionCategory>(_dbContext);
                return attractionCategoryRepository;
            }
        }

        public IGenericRepository<Booking> BookingRepository
        {
            get
            {
                bookingRepository ??= new GenericRepository<Booking>(_dbContext);
                return bookingRepository;
            }
        }

        public IGenericRepository<BookingPayment> BookingPaymentRepository
        {
            get
            {
                bookingPaymentRepository ??= new GenericRepository<BookingPayment>(_dbContext);
                return bookingPaymentRepository;
            }
        }

        public IGenericRepository<Customer> CustomerRepository
        {
            get
            {
                customerRepository ??= new GenericRepository<Customer>(_dbContext);
                return customerRepository;
            }
        }

        public IGenericRepository<EntityHistory> EntityHistoryRepository
        {
            get
            {
                entityHistoryRepository ??= new GenericRepository<EntityHistory>(_dbContext);
                return entityHistoryRepository;
            }
        }

        public IGenericRepository<EntityStatusHistory> EntityStatusHistoryRepository
        {
            get
            {
                entityStatusHistoryRepository ??= new GenericRepository<EntityStatusHistory>(_dbContext);
                return entityStatusHistoryRepository;
            }
        }

        public IGenericRepository<Event> EventRepository
        {
            get
            {
                eventRepository ??= new GenericRepository<Event>(_dbContext);
                return eventRepository;
            }
        }

        public IGenericRepository<EventCategory> EventCategoryRepository
        {
            get
            {
                eventCategoryRepository ??= new GenericRepository<EventCategory>(_dbContext);
                return eventCategoryRepository;
            }
        }

        public IGenericRepository<TourReview> FeedbackRepository
        {
            get
            {
                feedbackRepository ??= new GenericRepository<TourReview>(_dbContext);
                return feedbackRepository;
            }
        }

        public IGenericRepository<Manager> ManagerRepository
        {
            get
            {
                managerRepository ??= new GenericRepository<Manager>(_dbContext);
                return managerRepository;
            }
        }

        public IGenericRepository<Post> PostRepository
        {
            get
            {
                postRepository ??= new GenericRepository<Post>(_dbContext);
                return postRepository;
            }
        }

        public IGenericRepository<PostCategory> PostCategoryRepository
        {
            get
            {
                postCategoryRepository ??= new GenericRepository<PostCategory>(_dbContext);
                return postCategoryRepository;
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

        public IGenericRepository<Staff> StaffRepository
        {
            get
            {
                staffRepository ??= new GenericRepository<Staff>(_dbContext);
                return staffRepository;
            }
        }

        public IGenericRepository<Tour> TourRepository
        {
            get
            {
                tourRepository ??= new GenericRepository<Tour>(_dbContext);
                return tourRepository;
            }
        }

        public IGenericRepository<TourCategory> TourCategoryRepository
        {
            get
            {
                tourCategoryRepository ??= new GenericRepository<TourCategory>(_dbContext);
                return tourCategoryRepository;
            }
        }

        public IGenericRepository<TourDuration> TourDurationRepository
        {
            get
            {
                tourDurationRepository ??= new GenericRepository<TourDuration>(_dbContext);
                return tourDurationRepository;
            }
        }

        public IGenericRepository<TourTemplate> TourTemplateRepository
        {
            get
            {
                tourTemplateRepository ??= new GenericRepository<TourTemplate>(_dbContext);
                return tourTemplateRepository;
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
            transaction = await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }
                _dbContext.Dispose();
            }
            disposed = true;
        }
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (disposed)
            {
                return;
            }
            if (transaction != null)
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
            await _dbContext.DisposeAsync();
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
