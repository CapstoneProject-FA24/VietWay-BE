using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;


namespace VietWay.Repository.EntityModel
{
    public class TourBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long BookingId { get; set; }
        [ForeignKey(nameof(Tour))]
        public long TourId { get; set; }
        [ForeignKey(nameof(CustomerInfo))]
        public long CustomerId { get; set; }
        public BookingStatus Status { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual Customer? CustomerInfo { get; set; }
        public virtual BookingPayment? BookingPayment { get; set; }
        public virtual CustomerFeedback? CustomerFeedback { get; set; }
    }
}
