using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class BookingRefundDTO
    {
        public string? RefundId { get; set; }
        public string? BookingId { get; set; }
        public decimal RefundAmount { get; set; }
        public RefundStatus RefundStatus { get; set; }
        public DateTime? RefundDate { get; set; }
        public string? RefundReason { get; set; }
        public string? RefundNote { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
