using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class CreateManagerAccountRequest
    {
        public string? Email { get; set; }
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")]
        public required string Password { get; set; }
        [Required]
        public required string FullName { get; set; }
    }
}
