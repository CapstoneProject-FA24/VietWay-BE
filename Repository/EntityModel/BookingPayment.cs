using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class BookingPayment
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? PaymentId { get; set; }
        [ForeignKey(nameof(Booking))]
        [StringLength(20)]
        [Required]
        public string? BookingId { get; set; }
        [Range(0.00, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ThirdPartyTransactionNumber { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
