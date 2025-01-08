using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourTemplateReport
    {
        [Key]
        public string? ReportId { get; set; }
        [Required]
        public string? ReportLabel { get; set; }
        [Required]
        public ReportPeriod ReportPeriod { get; set; }
        [Required]
        [ForeignKey(nameof(Province))]
        [StringLength(20)]
        public string? ProvinceId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourCategory))]
        [Required]
        public string? TourCategoryId { get; set; }

        public int SiteReferralCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int TwitterReferralCount { get; set; }

        public int SiteLikeCount { get; set; }
        public int BookingCount { get; set; }
        public int CancellationCount { get; set; }
        public int FiveStarRatingCount { get; set; }
        public int FourStarRatingCount { get; set; }
        public int ThreeStarRatingCount { get; set; }
        public int TwoStarRatingCount { get; set; }
        public int OneStarRatingCount { get; set; }

        public int FacebookClickCount { get; set; }
        public int FacebookImpressionCount { get; set; }
        public int FacebookLikeCount { get; set; }
        public int FacebookLoveCount { get; set; }
        public int FacebookWowCount { get; set; }
        public int FacebookHahaCount { get; set; }
        public int FacebookSorryCount { get; set; }
        public int FacebookAngerCount { get; set; }
        public int FacebookShareCount { get; set; }
        public int FacebookCommentCount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int FacebookReactionCount { get; set; }

        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int BookmarkCount { get; set; }
        public int ImpressionCount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double FacebookScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TwitterScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double SiteScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double AverageScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double FacebookCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TwitterCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        public virtual Province? Province { get; set; }
        public virtual TourCategory? TourCategory { get; set; }
    }
}
