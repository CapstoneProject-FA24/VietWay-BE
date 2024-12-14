using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class StaffPreviewDTO
    {
        public string? StaffId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public required DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
