using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class PostMetric
    {
        [Key]
        [StringLength(20)]
        public string? MetricId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Post))]
        public string? PostId { get; set; }
        public int? NewViewCount { get; set; }
        public int? NewSaveCount { get; set; }
        public int? NewFacebookReferralCount { get; set; }
        public int? NewXReferralCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Score { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post? Post { get; set; }
    }
}
