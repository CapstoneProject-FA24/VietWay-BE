using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class AttractionMetric
    {
        [Key]
        [StringLength(20)]
        public string? MetricId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Attraction))]
        [Required]
        public string? AttractionId { get; set; }
        public int SiteReferralCount { get; set; }
        public int SiteLikeCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int XReferralCount { get; set; }
        public int FiveStarRatingCount { get; set; }
        public int FiveStarRatingLikeCount { get; set; }
        public int FourStarRatingCount { get; set; }
        public int FourStarRatingLikeCount { get; set; }
        public int ThreeStarRatingCount { get; set; }
        public int ThreeStarRatingLikeCount { get; set; }
        public int TwoStarRatingCount { get; set; }
        public int TwoStarRatingLikeCount { get; set; }
        public int OneStarRatingCount { get; set; }
        public int OneStarRatingLikeCount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Attraction? Attraction { get; set; }
    }
}
