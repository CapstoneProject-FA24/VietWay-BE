using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class VnPayIPNRequest
    {
        [Required]
        [FromQuery(Name = "vnp_TmnCode")]
        public required string TmnCode { get; set; }
        [Required]
        [FromQuery(Name = "vnp_Amount")]
        [Range(0, 999999999999)]
        public long Amount { get; set; }
        [Required]
        [FromQuery(Name = "vnp_BankCode")]
        public required string BankCode { get; set; }
        [FromQuery(Name = "vnp_BankTranNo")]
        public string? BankTranNo { get; set; }
        [FromQuery(Name = "vnp_CardType")]
        public string? CardType { get; set; }
        [FromQuery(Name = "vnp_PayDate")]
        public string? PayDate { get; set; }
        [Required]
        [FromQuery(Name = "vnp_OrderInfo")]
        public required string OrderInfo { get; set; }
        [Required]
        [FromQuery(Name = "vnp_TransactionNo")]
        public required string TransactionNo { get; set; }
        [Required]
        [FromQuery(Name = "vnp_ResponseCode")]
        public required string ResponseCode { get; set; }
        [Required]
        [FromQuery(Name = "vnp_TransactionStatus")]
        public required string TransactionStatus { get; set; }
        [Required]
        [FromQuery(Name = "vnp_TxnRef")]
        public required string TxnRef { get; set; }
        [FromQuery(Name = "vnp_SecureHashType")]
        public string? SecureHashType { get; set; }
        [Required]
        [FromQuery(Name = "vnp_SecureHash")]
        public required string SecureHash { get; set; }
    }
}
