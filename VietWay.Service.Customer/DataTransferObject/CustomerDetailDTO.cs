using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class CustomerDetailDTO
    {
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string ProvinceId { get; set; }
        public required string ProvinceName { get; set; }
        public Gender Gender { get; set; }
        public required bool LoginWithGoogle { get; set; }
    }
}
