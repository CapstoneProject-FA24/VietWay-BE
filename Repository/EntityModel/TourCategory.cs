using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourCategory : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string TourCategoryId { get; set; }
        [StringLength(255)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
