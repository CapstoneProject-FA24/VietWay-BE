using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<Attraction> AttractionRepository { get; }
        public IGenericRepository<AttractionCategory> AttractionCategoryRepository { get; }
        public IGenericRepository<AttractionLike> AttractionLikeRepository { get; }
        public IGenericRepository<AttractionMetric> AttractionMetricRepository { get; }
        public IGenericRepository<AttractionReport> AttractionReportRepository { get; }
        public IGenericRepository<AttractionReview> AttractionReviewRepository { get; }
        public IGenericRepository<AttractionReviewLike> AttractionReviewLikeRepository { get; }
        public IGenericRepository<Booking> BookingRepository { get; }
        public IGenericRepository<BookingPayment> BookingPaymentRepository { get; }
        public IGenericRepository<BookingRefund> BookingRefundRepository { get; }
        public IGenericRepository<Customer> CustomerRepository { get; }
        public IGenericRepository<EntityHistory> EntityHistoryRepository { get; }
        public IGenericRepository<EntityStatusHistory> EntityStatusHistoryRepository { get; }
        public IGenericRepository<FacebookPostMetric> FacebookPostMetricRepository { get; }
        public IGenericRepository<Hashtag> HashtagRepository { get; }
        public IGenericRepository<HashtagReport> HashtagReportRepository { get; }
        public IGenericRepository<Manager> ManagerRepository { get; }
        public IGenericRepository<Post> PostRepository { get; }
        public IGenericRepository<PostCategory> PostCategoryRepository { get; }
        public IGenericRepository<PostLike> PostLikeRepository { get; }
        public IGenericRepository<PostMetric> PostMetricRepository { get; }
        public IGenericRepository<PostReport> PostReportRepository { get; }
        public IGenericRepository<Province> ProvinceRepository { get; }
        public IGenericRepository<Staff> StaffRepository { get; }
        public IGenericRepository<SocialMediaPost> SocialMediaPostRepository { get; }
        public IGenericRepository<Tour> TourRepository { get; }
        public IGenericRepository<TourCategory> TourCategoryRepository { get; }
        public IGenericRepository<TourDuration> TourDurationRepository { get; }
        public IGenericRepository<TourReview> TourReviewRepository { get; }
        public IGenericRepository<TourTemplate> TourTemplateRepository { get; }
        public IGenericRepository<TourTemplateMetric> TourTemplateMetricRepository { get; }
        public IGenericRepository<TourTemplateProvince> TourTemplateProvinceRepository { get; }
        public IGenericRepository<TourTemplateReport> TourTemplateReportRepository { get; }
        public IGenericRepository<TourPrice> TourPriceRepository { get; }   
        public IGenericRepository<TourRefundPolicy> TourRefundPolicyRepository { get; }
        public IGenericRepository<TwitterPostMetric> TwitterPostMetricRepository { get; }
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
