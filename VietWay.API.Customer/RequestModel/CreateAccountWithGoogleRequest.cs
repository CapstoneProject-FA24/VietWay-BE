using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.RequestModel
{
    public class CreateAccountWithGoogleRequest
    {
        [Required]
        public required string IdToken { get; set; }
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string ProvinceId { get; set; }
    }
}
