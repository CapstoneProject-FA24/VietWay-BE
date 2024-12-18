using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSWebhookData
    {
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public required string Description { get; set; }
        public required string AccountNumber { get; set; }
        public required string Reference { get; set; }
        public required string TransactionDateTime { get; set; }
        public required string Currency { get; set; }
        public required string PaymentLinkId { get; set; }
        public required string Code { get; set; }
        public required string Desc { get; set; }
        public string? CounterAccountBankId { get; set; }
        public string? CounterAccountBankName { get; set; }
        public string? CounterAccountName { get; set; }
        public string? CounterAccountNumber { get; set; }
        public string? VirtualAccountName { get; set; }
        public required string VirtualAccountNumber { get; set; }
    }
}
