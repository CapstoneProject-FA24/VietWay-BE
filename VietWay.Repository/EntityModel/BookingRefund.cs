using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class BookingRefund
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string? RefundId { get; set; }
        [Required]
        [StringLength(20)]
        [ForeignKey(nameof(Booking))]
        public string? BookingId { get; set; }
        [Required]
        [Range(0.01, 999999999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }
        [Required]
        public RefundStatus RefundStatus { get; set; }
        public DateTime? RefundDate { get; set; }
        [Required]
        public string? RefundReason { get; set; }
        public string? RefundNote { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
