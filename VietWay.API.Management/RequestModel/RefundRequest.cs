using System.ComponentModel.DataAnnotations;
using VietWay.API.Management.RequestModel.CustomValidation;

namespace VietWay.API.Management.RequestModel
{
    public class RefundRequest
    {
        public string? Note { get; set; }
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string BankTransactionNumber { get; set; }
        [Required]
        [PastDate(ErrorMessage = "PayTime must be a date and time in the past.")]
        public DateTime PayTime { get; set; }
    }
}
