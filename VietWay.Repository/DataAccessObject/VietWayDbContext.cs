using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VietWay.Repository.EntityModel;

namespace VietWay.Repository.DataAccessObject
{
    public class VietWayDbContext(DatabaseConfig config) : DbContext()
    {
        private readonly DatabaseConfig _config = config;
        #region DbSets
        public DbSet<Account> Account { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionCategory> AttractionCategory { get; set; }
        public DbSet<AttractionImage> AttractionImage { get; set; }
        public DbSet<AttractionLike> AttractionLike { get; set; }
        public DbSet<AttractionMetric> AttractionMetric { get; set; }
        public DbSet<AttractionReview> AttractionReview { get; set; }
        public DbSet<AttractionReviewLike> AttractionReviewLike { get; set; }
        public DbSet<AttractionSchedule> AttractionSchedule { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingPayment> BookingPayment { get; set; }
        public DbSet<BookingRefund> BookingRefund { get; set; }
        public DbSet<BookingTourist> BookingTourist { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<EntityHistory> EntityHistory { get; set; }
        public DbSet<EntityStatusHistory> EntityStatusHistory { get; set; }
        public DbSet<FacebookPostMetric> FacebookPostMetric { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
        public DbSet<PostLike> PostLike { get; set; }
        public DbSet<PostMetric> PostMetric { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<SocialMediaPost> SocialMediaPost { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourCategory> TourCategory { get; set; }
        public DbSet<TourDuration> TourDuration { get; set; }
        public DbSet<TourPrice> TourPrice { get; set; }
        public DbSet<TourRefundPolicy> TourRefundPolicy { get; set; }
        public DbSet<TourReview> TourReview { get; set; }
        public DbSet<TourTemplate> TourTemplate { get; set; }
        public DbSet<TourTemplateImage> TourTemplateImage { get; set; }
        public DbSet<TourTemplateMetric> TourTemplateMetric { get; set; }
        public DbSet<TourTemplateProvince> TourTemplateProvince { get; set; }
        public DbSet<TourTemplateSchedule> TourTemplateSchedule { get; set; }
        public DbSet<TwitterPostMetric> TwitterPostMetric { get; set; }
        public DbSet<Hashtag> Hashtag { get; set; }
        public DbSet<SocialMediaPostHashtag> SocialMediaPostHashtag { get; set; }
        public DbSet<HashtagReport> HashtagReport { get; set; }
        public DbSet<PostReport> PostReport { get; set; }
        public DbSet<AttractionReport> AttractionReport { get; set; }
        public DbSet<TourTemplateReport> TourTemplateReport { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_config.ConnectionString)
                .LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<Account>()
                .HasIndex(x => x.PhoneNumber)
                .IsUnique();
            modelBuilder.Entity<AttractionReviewLike>()
                .HasKey(x => new { x.ReviewId, x.CustomerId });
            modelBuilder.Entity<AttractionReviewLike>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.AttractionReviewLikes)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AttractionReviewLike>()
                .HasOne(x => x.AttractionReview)
                .WithMany(x => x.AttractionReviewLikes)
                .HasForeignKey(x => x.ReviewId);
            modelBuilder.Entity<FacebookPostMetric>()
            .Property(x => x.Score)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([PostClickCount], 0)*1 +
                        COALESCE([ImpressionCount], 0)*0.5 +
                        COALESCE([LikeCount], 0)*1 + 
                        COALESCE([LoveCount], 0)*2 + 
                        COALESCE([WowCount], 0)*1.5 + 
                        COALESCE([HahaCount], 0)*1.5 + 
                        COALESCE([SorryCount], 0)*(-1) + 
                        COALESCE([AngerCount], 0)*(-2) + 
                        COALESCE([ShareCount], 0)*3 + 
                        COALESCE([CommentCount], 0)*2
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TwitterPostMetric>()
                .Property(x => x.Score)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([RetweetCount], 0)*3 +
                        COALESCE([ReplyCount], 0)*2 + 
                        COALESCE([LikeCount], 0)*1.5 + 
                        COALESCE([QuoteCount], 0)*3 + 
                        COALESCE([BookmarkCount], 0)*2 + 
                        COALESCE([ImpressionCount], 0)*0.5
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionMetric>()
                .Property(x => x.Score)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 + 
                        COALESCE([SiteLikeCount], 0)*5 + 
                        COALESCE([FacebookReferralCount], 0)*1 + 
                        COALESCE([XReferralCount], 0)*1 + 
                        COALESCE([FiveStarRatingCount], 0)*3 + 
                        COALESCE([FiveStarRatingLikeCount], 0)*3 + 
                        COALESCE([FourStarRatingCount], 0)*1 + 
                        COALESCE([FourStarRatingLikeCount], 0)*1 + 
                        COALESCE([ThreeStarRatingCount], 0)*0 + 
                        COALESCE([ThreeStarRatingLikeCount], 0)*0 + 
                        COALESCE([TwoStarRatingCount], 0)*(-1) + 
                        COALESCE([TwoStarRatingLikeCount], 0)*(-1) + 
                        COALESCE([OneStarRatingCount], 0)*(-3) + 
                        COALESCE([OneStarRatingLikeCount], 0)*(-3)
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostMetric>()
                .Property(x => x.Score)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 + 
                        COALESCE([SiteSaveCount], 0)*5 + 
                        COALESCE([FacebookReferralCount], 0)*1 + 
                        COALESCE([XReferralCount], 0)*1
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateMetric>()
                .Property(x => x.Score)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 + 
                        COALESCE([BookingCount], 0)*8 + 
                        COALESCE([CancellationCount], 0)*(-4) + 
                        COALESCE([FacebookReferralCount], 0)*1 + 
                        COALESCE([XReferralCount], 0)*1 + 
                        COALESCE([FiveStarRatingCount], 0)*3 + 
                        COALESCE([FourStarRatingCount], 0)*1 + 
                        COALESCE([ThreeStarRatingCount], 0)*0 + 
                        COALESCE([TwoStarRatingCount], 0)*(-1) + 
                        COALESCE([OneStarRatingCount], 0)*(-3)
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.FacebookReactionCount)
                .HasComputedColumnSql(@"
                    COALESCE([FacebookLikeCount], 0) + 
                    COALESCE([FacebookLoveCount], 0) + 
                    COALESCE([FacebookWowCount], 0) + 
                    COALESCE([FacebookHahaCount], 0) + 
                    COALESCE([FacebookSorryCount], 0) + 
                    COALESCE([FacebookAngerCount], 0)", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.FacebookCTR)
                .HasComputedColumnSql(@"
                    CAST(
	                    CASE
		                    WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0
		                    ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)
	                    END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.XCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.SiteScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 +
                        COALESCE([FacebookReferralCount], 0)*1 +
                        COALESCE([XReferralCount], 0)*1 +
                        COALESCE([SiteLikeCount], 0)*5 +
                        COALESCE([FiveStarRatingCount], 0)*3 +
                        COALESCE([FiveStarRatingLikeCount], 0)*3 +
                        COALESCE([FourStarRatingCount], 0)*1 +
                        COALESCE([FourStarRatingLikeCount], 0)*1 +
                        COALESCE([ThreeStarRatingCount], 0)*0 +
                        COALESCE([ThreeStarRatingLikeCount], 0)*0 +
                        COALESCE([TwoStarRatingCount], 0)*(-1) +
                        COALESCE([TwoStarRatingLikeCount], 0)*(-1) +
                        COALESCE([OneStarRatingCount], 0)*(-3) +
                        COALESCE([OneStarRatingLikeCount], 0)*(-3)
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.FacebookScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([FacebookClickCount], 0)*1 +
                        COALESCE([FacebookImpressionCount], 0)*0.5 +
                        COALESCE([FacebookLikeCount], 0)*1 + 
                        COALESCE([FacebookLoveCount], 0)*2 + 
                        COALESCE([FacebookWowCount], 0)*1.5 + 
                        COALESCE([FacebookHahaCount], 0)*1.5 + 
                        COALESCE([FacebookSorryCount], 0)*(-1) + 
                        COALESCE([FacebookAngerCount], 0)*(-2) + 
                        COALESCE([FacebookShareCount], 0)*3 + 
                        COALESCE([FacebookCommentCount], 0)*2
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.XScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([XRetweetCount], 0)*3 +
                        COALESCE([XReplyCount], 0)*2 + 
                        COALESCE([XLikeCount], 0)*1.5 + 
                        COALESCE([XQuoteCount], 0)*3 + 
                        COALESCE([XBookmarkCount], 0)*2 + 
                        COALESCE([XImpressionCount], 0)*0.5
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<AttractionReport>()
                .Property(x => x.AverageScore)
                .HasComputedColumnSql(@"
                    CAST(
                        (CAST(
                            COALESCE([FacebookClickCount], 0)*1 +
                            COALESCE([FacebookImpressionCount], 0)*0.5 +
                            COALESCE([FacebookLikeCount], 0)*1 + 
                            COALESCE([FacebookLoveCount], 0)*2 + 
                            COALESCE([FacebookWowCount], 0)*1.5 + 
                            COALESCE([FacebookHahaCount], 0)*1.5 + 
                            COALESCE([FacebookSorryCount], 0)*(-1) + 
                            COALESCE([FacebookAngerCount], 0)*(-2) + 
                            COALESCE([FacebookShareCount], 0)*3 + 
                            COALESCE([FacebookCommentCount], 0)*2
                        AS decimal(18,2)) +
                        CAST(
                            COALESCE([SiteReferralCount], 0)*2 +
                            COALESCE([FacebookReferralCount], 0)*1 +
                            COALESCE([XReferralCount], 0)*1 +
                            COALESCE([SiteLikeCount], 0)*5 +
                            COALESCE([FiveStarRatingCount], 0)*3 +
                            COALESCE([FiveStarRatingLikeCount], 0)*3 +
                            COALESCE([FourStarRatingCount], 0)*1 +
                            COALESCE([FourStarRatingLikeCount], 0)*1 +
                            COALESCE([ThreeStarRatingCount], 0)*0 +
                            COALESCE([ThreeStarRatingLikeCount], 0)*0 +
                            COALESCE([TwoStarRatingCount], 0)*(-1) +
                            COALESCE([TwoStarRatingLikeCount], 0)*(-1) +
                            COALESCE([OneStarRatingCount], 0)*(-3) +
                            COALESCE([OneStarRatingLikeCount], 0)*(-3)
                        AS decimal(18,2)) +
                        CAST(
                            COALESCE([XRetweetCount], 0)*3 +
                            COALESCE([XReplyCount], 0)*2 + 
                            COALESCE([XLikeCount], 0)*1.5 + 
                            COALESCE([XQuoteCount], 0)*3 + 
                            COALESCE([XBookmarkCount], 0)*2 + 
                            COALESCE([XImpressionCount], 0)*0.5
                        AS decimal(18,2))) / 3.00
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.FacebookReactionCount)
                .HasComputedColumnSql(@"
                    COALESCE([FacebookLikeCount], 0) + 
                    COALESCE([FacebookLoveCount], 0) + 
                    COALESCE([FacebookWowCount], 0) + 
                    COALESCE([FacebookHahaCount], 0) + 
                    COALESCE([FacebookSorryCount], 0) + 
                    COALESCE([FacebookAngerCount], 0)", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.FacebookCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.XCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.SiteScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 +
                        COALESCE([SiteLikeCount], 0)*5 +
                        COALESCE([FacebookReferralCount], 0)*1 +
                        COALESCE([XReferralCount], 0)*1
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.FacebookScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([FacebookClickCount], 0)*1 +
                        COALESCE([FacebookImpressionCount], 0)*0.5 +
                        COALESCE([FacebookLikeCount], 0)*1 + 
                        COALESCE([FacebookLoveCount], 0)*2 + 
                        COALESCE([FacebookWowCount], 0)*1.5 + 
                        COALESCE([FacebookHahaCount], 0)*1.5 + 
                        COALESCE([FacebookSorryCount], 0)*(-1) + 
                        COALESCE([FacebookAngerCount], 0)*(-2) + 
                        COALESCE([FacebookShareCount], 0)*3 + 
                        COALESCE([FacebookCommentCount], 0)*2
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.XScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([XRetweetCount], 0)*3 +
                        COALESCE([XReplyCount], 0)*2 + 
                        COALESCE([XLikeCount], 0)*1.5 + 
                        COALESCE([XQuoteCount], 0)*3 + 
                        COALESCE([XBookmarkCount], 0)*2 + 
                        COALESCE([XImpressionCount], 0)*0.5
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<PostReport>()
                .Property(x => x.AverageScore)
                .HasComputedColumnSql(@"
                    CAST(
                        (CAST(
                            COALESCE([SiteReferralCount], 0)*2 +
                            COALESCE([SiteLikeCount], 0)*5 +
                            COALESCE([FacebookReferralCount], 0)*1 +
                            COALESCE([XReferralCount], 0)*1
                        AS decimal(18,2)) + 
                        CAST(
                            COALESCE([FacebookClickCount], 0)*1 +
                            COALESCE([FacebookImpressionCount], 0)*0.5 +
                            COALESCE([FacebookLikeCount], 0)*1 + 
                            COALESCE([FacebookLoveCount], 0)*2 + 
                            COALESCE([FacebookWowCount], 0)*1.5 + 
                            COALESCE([FacebookHahaCount], 0)*1.5 + 
                            COALESCE([FacebookSorryCount], 0)*(-1) + 
                            COALESCE([FacebookAngerCount], 0)*(-2) + 
                            COALESCE([FacebookShareCount], 0)*3 + 
                            COALESCE([FacebookCommentCount], 0)*2
                        AS decimal(18,2)) + 
                        CAST(
                            COALESCE([XRetweetCount], 0)*3 +
                            COALESCE([XReplyCount], 0)*2 + 
                            COALESCE([XLikeCount], 0)*1.5 + 
                            COALESCE([XQuoteCount], 0)*3 + 
                            COALESCE([XBookmarkCount], 0)*2 + 
                            COALESCE([XImpressionCount], 0)*0.5
                        AS decimal(18,2))) / 3.00
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.FacebookReactionCount)
                .HasComputedColumnSql(@"
                    COALESCE([FacebookLikeCount], 0) + 
                    COALESCE([FacebookLoveCount], 0) + 
                    COALESCE([FacebookWowCount], 0) + 
                    COALESCE([FacebookHahaCount], 0) + 
                    COALESCE([FacebookSorryCount], 0) + 
                    COALESCE([FacebookAngerCount], 0)", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.FacebookCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.XCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.SiteScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([SiteReferralCount], 0)*2 +
                        COALESCE([BookingCount], 0)*8 +
                        COALESCE([CancellationCount], 0)*(-4) +
                        COALESCE([FacebookReferralCount], 0)*1 +
                        COALESCE([XReferralCount], 0)*1 +
                        COALESCE([FiveStarRatingCount], 0)*3 +
                        COALESCE([FourStarRatingCount], 0)*1 +
                        COALESCE([ThreeStarRatingCount], 0)*0 +
                        COALESCE([TwoStarRatingCount], 0)*(-1) +
                        COALESCE([OneStarRatingCount], 0)*(-3)
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.FacebookScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([FacebookClickCount], 0)*1 +
                        COALESCE([FacebookImpressionCount], 0)*0.5 +
                        COALESCE([FacebookLikeCount], 0)*1 + 
                        COALESCE([FacebookLoveCount], 0)*2 + 
                        COALESCE([FacebookWowCount], 0)*1.5 + 
                        COALESCE([FacebookHahaCount], 0)*1.5 + 
                        COALESCE([FacebookSorryCount], 0)*(-1) + 
                        COALESCE([FacebookAngerCount], 0)*(-2) + 
                        COALESCE([FacebookShareCount], 0)*3 + 
                        COALESCE([FacebookCommentCount], 0)*2
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.XScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([XRetweetCount], 0)*3 +
                        COALESCE([XReplyCount], 0)*2 + 
                        COALESCE([XLikeCount], 0)*1.5 + 
                        COALESCE([XQuoteCount], 0)*3 + 
                        COALESCE([XBookmarkCount], 0)*2 + 
                        COALESCE([XImpressionCount], 0)*0.5
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<TourTemplateReport>()
                .Property(x => x.AverageScore)
                .HasComputedColumnSql(@"
                    CAST(
                        (CAST(
                            COALESCE([SiteReferralCount], 0)*2 +
                            COALESCE([BookingCount], 0)*8 +
                            COALESCE([CancellationCount], 0)*(-4) +
                            COALESCE([FacebookReferralCount], 0)*1 +
                            COALESCE([XReferralCount], 0)*1 +
                            COALESCE([FiveStarRatingCount], 0)*3 +
                            COALESCE([FourStarRatingCount], 0)*1 +
                            COALESCE([ThreeStarRatingCount], 0)*0 +
                            COALESCE([TwoStarRatingCount], 0)*(-1) +
                            COALESCE([OneStarRatingCount], 0)*(-3)
                        AS decimal(18,2)) + 
                        CAST(
                            COALESCE([FacebookClickCount], 0)*1 +
                            COALESCE([FacebookImpressionCount], 0)*0.5 +
                            COALESCE([FacebookLikeCount], 0)*1 + 
                            COALESCE([FacebookLoveCount], 0)*2 + 
                            COALESCE([FacebookWowCount], 0)*1.5 + 
                            COALESCE([FacebookHahaCount], 0)*1.5 + 
                            COALESCE([FacebookSorryCount], 0)*(-1) + 
                            COALESCE([FacebookAngerCount], 0)*(-2) + 
                            COALESCE([FacebookShareCount], 0)*3 + 
                            COALESCE([FacebookCommentCount], 0)*2
                        AS decimal(18,2)) + 
                        CAST(
                            COALESCE([XRetweetCount], 0)*3 +
                            COALESCE([XReplyCount], 0)*2 + 
                            COALESCE([XLikeCount], 0)*1.5 + 
                            COALESCE([XQuoteCount], 0)*3 + 
                            COALESCE([XBookmarkCount], 0)*2 + 
                            COALESCE([XImpressionCount], 0)*0.5
                        AS decimal(18,2))) / 3.00
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<HashtagReport>()
                .Property(x => x.FacebookReactionCount)
                .HasComputedColumnSql(@"
                    COALESCE([FacebookLikeCount], 0) + 
                    COALESCE([FacebookLoveCount], 0) + 
                    COALESCE([FacebookWowCount], 0) + 
                    COALESCE([FacebookHahaCount], 0) + 
                    COALESCE([FacebookSorryCount], 0) + 
                    COALESCE([FacebookAngerCount], 0)", stored: true);
            modelBuilder.Entity<HashtagReport>()
                .Property(x => x.FacebookCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<HashtagReport>()
                .Property(x => x.XCTR)
                .HasComputedColumnSql(@"
                    CAST(
                        CASE
                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0
                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)
                        END 
                    AS decimal(18,2))", stored: true);
            modelBuilder.Entity<HashtagReport>()
                .Property(x => x.FacebookScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([FacebookClickCount], 0)*1 +
                        COALESCE([FacebookImpressionCount], 0)*0.5 +
                        COALESCE([FacebookLikeCount], 0)*1 + 
                        COALESCE([FacebookLoveCount], 0)*2 + 
                        COALESCE([FacebookWowCount], 0)*1.5 + 
                        COALESCE([FacebookHahaCount], 0)*1.5 + 
                        COALESCE([FacebookSorryCount], 0)*(-1) + 
                        COALESCE([FacebookAngerCount], 0)*(-2) + 
                        COALESCE([FacebookShareCount], 0)*3 + 
                        COALESCE([FacebookCommentCount], 0)*2
                    AS decimal(18,2))", stored:true);
            modelBuilder.Entity<HashtagReport>()
                .Property(x => x.XScore)
                .HasComputedColumnSql(@"
                    CAST(
                        COALESCE([XRetweetCount], 0)*3 +
                        COALESCE([XReplyCount], 0)*2 + 
                        COALESCE([XLikeCount], 0)*1.5 + 
                        COALESCE([XQuoteCount], 0)*3 + 
                        COALESCE([XBookmarkCount], 0)*2 + 
                        COALESCE([XImpressionCount], 0)*0.5
                    AS decimal(18,2))", stored: true);
        }
    }
}
