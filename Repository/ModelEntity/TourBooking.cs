using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VietWay.Repository.ModelEntity
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
        public virtual CustomerInfo? CustomerInfo { get; set; }
        public virtual BookingPayment? BookingPayment { get; set; }
        public virtual CustomerFeedback? CustomerFeedback { get; set; }
    }
}
