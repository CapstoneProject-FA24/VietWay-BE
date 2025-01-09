using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class TourTemplateMetric
    {
        [Key]
        [StringLength(20)]
        public string? MetricId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourTemplate))]
        public string? TourTemplateId { get; set; }
        public int SiteReferralCount { get; set; }
        public int BookingCount { get; set; }
        public int CancellationCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int XReferralCount { get; set; }
        public int FiveStarRatingCount { get; set; }
        public int FourStarRatingCount { get; set; }
        public int ThreeStarRatingCount { get; set; }
        public int TwoStarRatingCount { get; set; }
        public int OneStarRatingCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
