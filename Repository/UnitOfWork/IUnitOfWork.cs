using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<Admin> AdminRepository { get; }
        public IGenericRepository<Attraction> AttractionRepository { get; }
        public IGenericRepository<AttractionCategory> AttractionCategoryRepository { get; }
        public IGenericRepository<AttractionLike> AttractionLikeRepository { get; }
        public IGenericRepository<AttractionReview> AttractionReviewRepository { get; }
        public IGenericRepository<AttractionReviewLike> AttractionReviewLikeRepository { get; }
        public IGenericRepository<Booking> BookingRepository { get; }
        public IGenericRepository<BookingPayment> BookingPaymentRepository { get; }
        public IGenericRepository<Customer> CustomerRepository { get; }
        public IGenericRepository<EntityHistory> EntityHistoryRepository { get; }
        public IGenericRepository<EntityStatusHistory> EntityStatusHistoryRepository { get; }
        public IGenericRepository<TourReview> FeedbackRepository { get; }
        public IGenericRepository<Manager> ManagerRepository { get; }
        public IGenericRepository<Post> PostRepository { get; }
        public IGenericRepository<PostCategory> PostCategoryRepository { get; }
        public IGenericRepository<Province> ProvinceRepository { get; }
        public IGenericRepository<Staff> StaffRepository { get; }
        public IGenericRepository<Tour> TourRepository { get; }
        public IGenericRepository<TourCategory> TourCategoryRepository { get; }
        public IGenericRepository<TourDuration> TourDurationRepository { get; }
        public IGenericRepository<TourTemplate> TourTemplateRepository { get; }

        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
