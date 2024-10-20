using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class LoginRequest
    {

        [Required]
        public required string EmailOrPhone { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
