using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class CreateStaffAccountRequest
    {
        public string? Email { get; set; }
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string FullName { get; set; }
    }
}
