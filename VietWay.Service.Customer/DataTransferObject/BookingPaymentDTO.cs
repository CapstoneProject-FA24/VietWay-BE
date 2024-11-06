using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class BookingPaymentDTO
    {
        public required string PaymentId { get; set; }
        public required string BookingId { get; set; }
        public required decimal Amount { get; set; }
        public string? Note { get; set; }
        public required DateTime CreateAt { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ThirdPartyTransactionNumber { get; set; }
        public required PaymentStatus Status { get; set; }
    }
}
