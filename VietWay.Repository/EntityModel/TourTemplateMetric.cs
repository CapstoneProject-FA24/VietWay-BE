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
        public int? NewViewCount { get; set; }
        public int? NewBookingCount { get; set; }
        public int? NewCancellationCount { get; set; }
        public int? NewFacebookReferralCount { get; set; }
        public int? NewXReferralCount { get; set; }
        public int? New5StarRatingCount { get; set; }
        public int? New4StarRatingCount { get; set; }
        public int? New3StarRatingCount { get; set; }
        public int? New2StarRatingCount { get; set; }
        public int? New1StarRatingCount { get; set; }
        public int? Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
