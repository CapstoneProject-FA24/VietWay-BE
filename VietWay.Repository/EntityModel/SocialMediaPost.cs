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
        [ForeignKey(nameof(Attraction))]
        [StringLength(20)]
        public string? AttractionId { get; set; }
        [ForeignKey(nameof(Post))]
        [StringLength(20)]
        public string? PostId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(TourTemplate))]
        public string? TourTemplateId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<FacebookPostMetric>? FacebookPostMetrics { get; set; }
        public virtual ICollection<TwitterPostMetric>? TwitterPostMetrics { get; set; }
        public virtual Attraction? Attraction { get; set; }
        public virtual Post? Post { get; set; }
        public virtual TourTemplate? TourTemplate { get; set; }
    }
}
