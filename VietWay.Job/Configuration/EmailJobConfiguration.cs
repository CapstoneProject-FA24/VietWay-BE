using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Job.Configuration
{
    public class EmailJobConfiguration
    {
        public required string ConfirmBookingTemplate { get; set; }
        public required string CancelBookingTemplate { get; set; }
    }
}
