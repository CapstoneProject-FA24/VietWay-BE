using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class BookingHistoryDTO
    {
        public UserRole ModifierRole { get; set; }
        public string? Reason { get; set; }
        public EntityModifyAction Action { get; set; }
        public DateTime Timestamp { get; set; }
        public int? OldStatus { get; set; }
        public int? NewStatus { get; set; }
    }
}
