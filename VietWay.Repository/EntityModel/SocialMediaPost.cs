using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class SocialMediaPost
    {
        [Key]
        public string? SocialPostId { get; set; }
        public SocialMediaSite Site { get; set; }
        public SocialMediaPostEntity EntityType { get; set; }
        [Required]
        [StringLength(20)]
        public string? EntityId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<FacebookPostMetric>? FacebookPostMetrics { get; set; }
        public virtual ICollection<TwitterPostMetric>? TwitterPostMetrics { get; set; }
    }
}
