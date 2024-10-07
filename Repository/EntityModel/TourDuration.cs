using System.ComponentModel.DataAnnotations;

namespace VietWay.Repository.EntityModel
{
    public class TourDuration
    {
        [Key]
        [StringLength(20)]
        public required string DurationId { get; set; }
        [StringLength(100)]
        public required string DurationName { get; set; }
        public required int NumberOfDay { get; set; }
    }
}
