using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class StaffDetailDTO
    {
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string FullName { get; set; }
    }
}
