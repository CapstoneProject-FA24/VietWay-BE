using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class FacebookPostMetric
    {
        [Key]
        public string? MetricId { get; set; }
        [ForeignKey(nameof(SocialMediaPost))]
        [Required]
        public string? SocialPostId { get; set; }
        public int PostClickCount { get; set; }
        public int ImpressionCount { get; set; }
        public int LikeCount { get; set; }
        public int LoveCount { get; set; }
        public int WowCount { get; set; }
        public int HahaCount { get; set; }
        public int SorryCount { get; set; }
        public int AngerCount { get; set; }
        public int ShareCount { get; set; }
        public int CommentCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double Score { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual SocialMediaPost? SocialMediaPost { get; set; }
    }
}
