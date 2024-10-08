using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class LoginByEmailRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
