using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourDuration : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? DurationId { get; set; }
        [StringLength(100)]
        [Required]
        public string? DurationName { get; set; }
        [Required]
        public int NumberOfDay { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
