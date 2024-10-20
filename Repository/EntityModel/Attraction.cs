using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Attraction : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string AttractionId { get; set; }
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(500)]
        public string? ContactInfo { get; set; }
        [StringLength(2048)]
        public string? Website { get; set; }
        public string? Description { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(ProvinceId))]
        public string? ProvinceId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(AttractionCategory))]
        public string? AttractionCategoryId { get; set; }
        [StringLength(50)]
        public string? GooglePlaceId { get; set; }
        public required AttractionStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual Province? Province { get; set; }
        public virtual AttractionCategory? AttractionCategory { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
