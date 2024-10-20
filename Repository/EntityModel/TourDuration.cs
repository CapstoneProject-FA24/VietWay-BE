using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourDuration : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string DurationId { get; set; }
        [StringLength(100)]
        public required string DurationName { get; set; }
        public required int NumberOfDay { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
