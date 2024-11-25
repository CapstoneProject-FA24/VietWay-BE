using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourCategory : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? TourCategoryId { get; set; }
        [StringLength(255)]
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<TourTemplate>? TourTemplates { get; set; }
    }
}
