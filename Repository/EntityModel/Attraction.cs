using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Attraction : CreatedByEntity<Staff>
    {
        [Key]
        [StringLength(20)]
        public required string AttractionId { get; set; }
        [StringLength(255)]
        public required string Name { get; set; }
        [StringLength(255)]
        public required string Address { get; set; }
        [StringLength(500)]
        public required string ContactInfo { get; set; }
        [StringLength(2048)]
        public string? Website { get; set; }
        public required string Description { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(ProvinceId))]
        public required string ProvinceId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(AttractionType))]
        public required string AttractionTypeId { get; set; }
        [StringLength(50)]
        public string? GooglePlaceId { get; set; }
        public required AttractionStatus Status { get; set; }

        public virtual Province? Province { get; set; }
        public virtual AttractionType? AttractionType { get; set; }
        public virtual ICollection<AttractionImage>? AttractionImages { get; set; }
    }
}
