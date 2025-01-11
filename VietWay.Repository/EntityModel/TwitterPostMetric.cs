using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class TwitterPostMetric
    {
        [Key]
        public string? MetricId { get; set; }
        [Required]
        [ForeignKey(nameof(SocialMediaPost))]
        public string? SocialPostId { get; set; }
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int BookmarkCount { get; set; }
        public int ImpressionCount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Score { get; }
        public required DateTime CreatedAt { get; set; }

        public virtual SocialMediaPost? SocialMediaPost { get; set; }
    }
}
