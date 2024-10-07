using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.ResponseModel
{
    public class CustomerProfile
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public Gender Gender { get; set; }
    }
}
