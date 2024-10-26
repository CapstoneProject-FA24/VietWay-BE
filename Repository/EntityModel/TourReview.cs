using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class TourReview : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string ReviewId { get; set; }
        [ForeignKey(nameof(Booking))]
        [StringLength(20)]
        public required string BookingId { get; set; }
        public required int Rating { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
