using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class ConfirmOtpRequest
    {
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        public required string Otp { get; set; }
    }
}
