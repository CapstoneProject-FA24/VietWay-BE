using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;


namespace VietWay.Repository.EntityModel
{
    public class TourBooking
    {
        [Key]
        [StringLength(20)]
        public required string BookingId { get; set; }
        [ForeignKey(nameof(Tour))]
        [StringLength(20)]
        public required string TourId { get; set; }
        [ForeignKey(nameof(CustomerInfo))]
        [StringLength(20)]
        public required string CustomerId { get; set; }
        public required BookingStatus Status { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual Customer? CustomerInfo { get; set; }
        public virtual BookingPayment? BookingPayment { get; set; }
        public virtual CustomerFeedback? CustomerFeedback { get; set; }
    }
}
