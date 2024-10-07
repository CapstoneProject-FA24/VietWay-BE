using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourCategory : CreatedByEntity<Manager>
    {
        [Key]
        [StringLength(20)]
        public required string TourCategoryId { get; set; }
        [StringLength(255)]
        public required string Name { get; set; }
    }
}
