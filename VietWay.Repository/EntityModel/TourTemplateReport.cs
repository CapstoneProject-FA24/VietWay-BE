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
        public DateTime CreatedAt { get; set; }

        public int SiteReferralCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int XReferralCount { get; set; }

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
        public int FacebookReactionCount { get; }

        public int XRetweetCount { get; set; }
        public int XReplyCount { get; set; }
        public int XLikeCount { get; set; }
        public int XQuoteCount { get; set; }
        public int XBookmarkCount { get; set; }
        public int XImpressionCount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FacebookScore { get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal XScore { get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SiteScore { get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageScore { get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FacebookCTR { get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal XCTR { get; }

        public virtual Province? Province { get; set; }
        public virtual TourCategory? TourCategory { get; set; }
    }
}
