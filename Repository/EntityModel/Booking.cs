using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;


namespace VietWay.Repository.EntityModel
{
    public class Booking
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? BookingId { get; set; }
        [ForeignKey(nameof(Tour))]
        [StringLength(20)]
        [Required]
        public string? TourId { get; set; }
        [ForeignKey(nameof(CustomerInfo))]
        [StringLength(20)]
        [Required]
        public string? CustomerId { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int NumberOfParticipants { get; set; }
        [StringLength(100)]
        [Required]
        public string? ContactFullName { get; set; }
        [StringLength(320)]
        [Required]
        public string? ContactEmail { get; set; }
        [StringLength(10)]
        [Required]
        public string? ContactPhoneNumber { get; set; }
        [StringLength(255)]
        public string? ContactAddress { get; set; }
        [Range(0.01, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public BookingStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string? Note { get; set; }

        public virtual Tour? Tour { get; set; }
        public virtual Customer? CustomerInfo { get; set; }
        public virtual TourReview? CustomerFeedback { get; set; }

        public virtual ICollection<BookingPayment>? BookingPayments { get; set; }
        public virtual ICollection<BookingTourist>? BookingTourists { get; set; }
    }
}
