using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;


namespace VietWay.Repository.EntityModel
{
    public class Booking
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
        [Range(1, int.MaxValue)]
        public required int NumberOfParticipants { get; set; }
        [StringLength(100)]
        public required string ContactFullName { get; set; }
        [StringLength(320)]
        public required string ContactEmail { get; set; }
        [StringLength(10)]
        public required string ContactPhoneNumber { get; set; }
        [StringLength(255)]
        public string? ContactAddress { get; set; }
        [Range(0.01, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal TotalPrice { get; set; }
        public required BookingStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public string? Note { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual Customer? CustomerInfo { get; set; }
        public virtual Feedback? CustomerFeedback { get; set; }

        public virtual ICollection<BookingPayment>? BookingPayments { get; set; }
        public virtual ICollection<BookingTourParticipant>? BookingTourParticipants { get; set; }
    }
}
