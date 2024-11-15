using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourReview : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? ReviewId { get; set; }
        [ForeignKey(nameof(Booking))]
        [StringLength(20)]
        [Required]
        public string? BookingId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string? Review { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsPublic { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
