using System.ComponentModel.DataAnnotations;
namespace VietWay.API.Customer.RequestModel
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "REQUIRED")]
        public required string EmailOrPhone { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public required string Password { get; set; }
    }
}
