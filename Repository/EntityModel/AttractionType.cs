using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class AttractionType : CreatedByEntity<Manager>
    {
        [Key]
        [StringLength(20)]
        public required string AttractionTypeId { get; set; }
        [StringLength(100)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<Attraction>? Attractions { get; set; }
    }
}
