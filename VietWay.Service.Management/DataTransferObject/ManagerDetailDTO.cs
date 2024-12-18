using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ManagerDetailDTO
    {
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string FullName { get; set; }
    }
}
