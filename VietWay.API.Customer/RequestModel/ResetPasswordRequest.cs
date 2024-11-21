using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class ResetPasswordRequest
    {
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")]
        public required string NewPassword { get; set; }
    }
}
