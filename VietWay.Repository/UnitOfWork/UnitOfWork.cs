using Microsoft.EntityFrameworkCore.Storage;
using VietWay.Repository.DataAccessObject;
using VietWay.Repository.EntityModel;
using VietWay.Repository.GenericRepository;

namespace VietWay.Repository.UnitOfWork
{
    public class UnitOfWork(DatabaseConfig config) : IUnitOfWork
    {
        private readonly VietWayDbContext _dbContext = new(config);

        public IDbContextTransaction? transaction;

        private IGenericRepository<Account>? accountRepository;
        private IGenericRepository<Attraction>? attractionRepository;
        private IGenericRepository<AttractionCategory>? attractionCategoryRepository;
        private IGenericRepository<AttractionLike>? attractionLikeRepository;
        private IGenericRepository<AttractionReview>? attractionReviewRepository;
        private IGenericRepository<AttractionReviewLike>? attractionReviewLikeRepository;
        private IGenericRepository<Booking>? bookingRepository;
        private IGenericRepository<BookingPayment>? bookingPaymentRepository;
        private IGenericRepository<BookingRefund>? bookingRefundRepository;
        private IGenericRepository<Customer>? customerRepository;
        private IGenericRepository<EntityHistory>? entityHistoryRepository;
        private IGenericRepository<EntityStatusHistory>? entityStatusHistoryRepository;
        private IGenericRepository<Manager>? managerRepository;
        private IGenericRepository<Post>? postRepository;
        private IGenericRepository<PostCategory>? postCategoryRepository;
        private IGenericRepository<PostLike>? postLikeRepository;
        private IGenericRepository<Province>? provinceRepository;
        private IGenericRepository<Staff>? staffRepository;
        private IGenericRepository<Tour>? tourRepository;
        private IGenericRepository<TourCategory>? tourCategoryRepository;
        private IGenericRepository<TourDuration>? tourDurationRepository;
        private IGenericRepository<TourReview>? tourReviewRepository;
        private IGenericRepository<TourTemplate>? tourTemplateRepository;
        private IGenericRepository<TourTemplateProvince>? tourTemplateProvinceRepository;
        private IGenericRepository<TourPrice>? tourPriceRepository;
        private IGenericRepository<TourRefundPolicy>? tourRefundPolicyRepository;
        private IGenericRepository<FacebookPostMetric>? facebookPostMetricRepository;
        private IGenericRepository<SocialMediaPost>? socialMediaPostRepository;
        private IGenericRepository<TwitterPostMetric>? twitterPostMetricRepository;
        private IGenericRepository<AttractionMetric>? attractionMetricRepository;
        private IGenericRepository<PostMetric>? postMetricRepository;
        private IGenericRepository<TourTemplateMetric>? tourTemplateMetricRepository;
        private IGenericRepository<SocialMediaPostHashtag>? socialMediaPostHashtagRepository;
        private IGenericRepository<AttractionReport>? attractionReportRepository;
        private IGenericRepository<Hashtag>? hashtagRepository;
        private IGenericRepository<HashtagReport>? hashtagReportRepository;
        private IGenericRepository<PostReport>? postReportRepository;
        private IGenericRepository<TourTemplateReport>? tourTemplateReportRepository;

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                accountRepository ??= new GenericRepository<Account>(_dbContext);
                return accountRepository;
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

        public IGenericRepository<AttractionLike> AttractionLikeRepository
        {
            get
            {
                attractionLikeRepository ??= new GenericRepository<AttractionLike>(_dbContext);
                return attractionLikeRepository;
            }
        }

        public IGenericRepository<AttractionReview> AttractionReviewRepository
        {
            get
            {
                attractionReviewRepository ??= new GenericRepository<AttractionReview>(_dbContext);
                return attractionReviewRepository;
            }
        }

        public IGenericRepository<AttractionReviewLike> AttractionReviewLikeRepository
        {
            get
            {
                attractionReviewLikeRepository ??= new GenericRepository<AttractionReviewLike>(_dbContext);
                return attractionReviewLikeRepository;
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
        public IGenericRepository<PostLike> PostLikeRepository
        {
            get
            {
                postLikeRepository ??= new GenericRepository<PostLike>(_dbContext);
                return postLikeRepository;
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

        public IGenericRepository<TourReview> TourReviewRepository
        {
            get
            {
                tourReviewRepository ??= new GenericRepository<TourReview>(_dbContext);
                return tourReviewRepository;
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
        public IGenericRepository<TourTemplateProvince> TourTemplateProvinceRepository
        {
            get
            {
                tourTemplateProvinceRepository ??= new GenericRepository<TourTemplateProvince>(_dbContext);
                return tourTemplateProvinceRepository;
            }
        }
        public IGenericRepository<TourPrice> TourPriceRepository
        {
            get
            {
                tourPriceRepository ??= new GenericRepository<TourPrice>(_dbContext);
                return tourPriceRepository;
            }
        }
        public IGenericRepository<TourRefundPolicy> TourRefundPolicyRepository
        {
            get
            {
                tourRefundPolicyRepository ??= new GenericRepository<TourRefundPolicy>(_dbContext);
                return tourRefundPolicyRepository;
            }
        }

        public IGenericRepository<BookingRefund> BookingRefundRepository 
        {
            get
            {
                bookingRefundRepository ??= new GenericRepository<BookingRefund>(_dbContext);
                return bookingRefundRepository;
            }
        }

        public IGenericRepository<FacebookPostMetric> FacebookPostMetricRepository{
            get
            {
                facebookPostMetricRepository ??= new GenericRepository<FacebookPostMetric>(_dbContext);
                return facebookPostMetricRepository;
            }
        }

        public IGenericRepository<SocialMediaPost> SocialMediaPostRepository
        { 
            get
            {
                socialMediaPostRepository ??= new GenericRepository<SocialMediaPost>(_dbContext);
                return socialMediaPostRepository;
            }
        }

        public IGenericRepository<TwitterPostMetric> TwitterPostMetricRepository
        {
            get
            {
                twitterPostMetricRepository ??= new GenericRepository<TwitterPostMetric>(_dbContext);
                return twitterPostMetricRepository;
            }
        }

        public IGenericRepository<AttractionMetric> AttractionMetricRepository
        {
            get
            {
                attractionMetricRepository ??= new GenericRepository<AttractionMetric>(_dbContext);
                return attractionMetricRepository;
            }
        }

        public IGenericRepository<PostMetric> PostMetricRepository
        {
            get
            {
                postMetricRepository ??= new GenericRepository<PostMetric>(_dbContext);
                return postMetricRepository;
            }
        }

        public IGenericRepository<TourTemplateMetric> TourTemplateMetricRepository
        {
            get
            {
                tourTemplateMetricRepository ??= new GenericRepository<TourTemplateMetric>(_dbContext);
                return tourTemplateMetricRepository;
            }
        }

        public IGenericRepository<AttractionReport> AttractionReportRepository 
        {
            get
            {
                attractionReportRepository ??= new GenericRepository<AttractionReport>(_dbContext);
                return attractionReportRepository;
            }
        }

        public IGenericRepository<Hashtag> HashtagRepository
        {
            get
            {
                hashTagRepository ??= new GenericRepository<Hashtag>(_dbContext);
                return hashTagRepository;
            }
        }

        public IGenericRepository<SocialMediaPostHashtag> SocialMediaPostHashtagRepository
        {
            get
            {
                socialMediaPostHashtagRepository ??= new GenericRepository<SocialMediaPostHashtag>(_dbContext);
                return socialMediaPostHashtagRepository;
            }
        }

        public IGenericRepository<HashtagReport> HashtagReportRepository
        {
            get
            {
                hashtagReportRepository ??= new GenericRepository<HashtagReport>(_dbContext);
                return hashtagReportRepository;
            }
        }

        public IGenericRepository<PostReport> PostReportRepository 
        {
            get
            {
                postReportRepository ??= new GenericRepository<PostReport>(_dbContext);
                return postReportRepository;
            }
        }

        public IGenericRepository<TourTemplateReport> TourTemplateReportRepository
        {
            get
            {
                tourTemplateReportRepository ??= new GenericRepository<TourTemplateReport>(_dbContext);
                return tourTemplateReportRepository;
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
