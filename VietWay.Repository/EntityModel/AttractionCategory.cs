using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class AttractionCategory : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? AttractionCategoryId { get; set; }
        [StringLength(100)]
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Attraction>? Attractions { get; set; }
        public virtual ICollection<AttractionReport>? AttractionReports { get; set; }
    }
}
