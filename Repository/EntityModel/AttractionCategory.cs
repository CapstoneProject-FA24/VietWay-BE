using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class AttractionCategory : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string AttractionCategoryId { get; set; }
        [StringLength(100)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
