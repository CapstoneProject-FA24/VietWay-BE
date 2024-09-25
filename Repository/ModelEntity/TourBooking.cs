using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repository.ModelEntity
{
    public class TourBooking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [ForeignKey(nameof(Tour))]
        public int TourId { get; set; }
        [ForeignKey(nameof(CustomerInfo))]
        public int CustomerId { get; set; }
        public Enum.BookingStatus Status { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual CustomerInfo? CustomerInfo { get; set; }
        public virtual BookingPayment? BookingPayment { get; set; }
        public virtual CustomerFeedback? CustomerFeedback { get; set; }
    }
}
