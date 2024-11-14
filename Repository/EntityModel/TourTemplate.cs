using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourTemplate : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? TourTemplateId { get; set; }
        [StringLength(20)]
        public string? Code { get; set; }
        [StringLength(500)]
        public string? TourName { get; set; }
        public string? Description { get; set; }
        [ForeignKey(nameof(TourDuration))]
        public string? DurationId { get; set; }
        [ForeignKey(nameof(TourCategory))]
        public string? TourCategoryId { get; set; }
        public string? Note { get; set; }
        [Required]
        public TourTemplateStatus Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxPrice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual TourDuration? TourDuration { get; set; }
        public virtual TourCategory? TourCategory { get; set; }
        public virtual ICollection<Tour>? Tours { get; set; }
        public virtual ICollection<TourTemplateProvince>? TourTemplateProvinces { get; set; }
        public virtual ICollection<TourTemplateSchedule>? TourTemplateSchedules { get; set; }
        public virtual ICollection<TourTemplateImage>? TourTemplateImages { get; set; }
    }
}
