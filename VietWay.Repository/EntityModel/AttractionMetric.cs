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
        public int? NewViewCount { get; set; }
        public int? NewLikeCount { get; set; }
        public int? NewFacebookReferralCount { get; set; }
        public int? NewXReferralCount { get; set; }
        public int? New5StarRatingCount { get; set; }
        public int? New5StarRatingLikeCount { get; set; }
        public int? New4StarRatingCount { get; set; }
        public int? New4StarRatingLikeCount { get; set; }
        public int? New3StarRatingCount { get; set; }
        public int? New3StarRatingLikeCount { get; set; }
        public int? New2StarRatingCount { get; set; }
        public int? New2StarRatingLikeCount { get; set; }
        public int? New1StarRatingCount { get; set; }
        public int? New1StarRatingLikeCount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Score { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Attraction? Attraction { get; set; }
    }
}
