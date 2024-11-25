using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Customer.RequestModel
{
    public class UpdateCustomerProfileRequest
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProvinceId { get; set; }
        public Gender? Gender { get; set; }
        public string? Email { get; set; }
    }
}
