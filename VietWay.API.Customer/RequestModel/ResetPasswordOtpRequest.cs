using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class ResetPasswordOtpRequest
    {
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
    }
}
