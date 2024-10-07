using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.DataTransferObject
{
    public class VnPayIPN
    {
        public required string TmnCode { get; set; }
        public long Amount { get; set; }
        public required string BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { get; set; }
        public string? PayDate { get; set; }
        public required string OrderInfo { get; set; }
        public required string TransactionNo { get; set; }
        public required string ResponseCode { get; set; }
        public required string TransactionStatus { get; set; }
        public required string TxnRef { get; set; }
        public string? SecureHashType { get; set; }
        public required string SecureHash { get; set; }
    }
}
