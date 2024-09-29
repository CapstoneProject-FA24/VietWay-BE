using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace VietWay.Repository.ModelEntity
{
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }
        [ForeignKey(nameof(BookingPayment))]
        public long PaymentId { get; set; }
        [Required, Range(0.00, 999999999999.99), Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public required string Note { get; set; }
        [Required]
        public required DateTime CreateOn { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ThirdPartyTransactionNumber { get; set; }
        public TransactionStatus Status { get; set; }

        public virtual BookingPayment? BookingPayment { get; set; }
    }
}
