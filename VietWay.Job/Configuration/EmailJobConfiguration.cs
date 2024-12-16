using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Job.Configuration
{
    public class EmailJobConfiguration(string cancelBookingTemplate, string confirmBookingTemplate)
    {
        private string _cancelBookingTemplate = cancelBookingTemplate;
        private string _confirmBookingTemplate = confirmBookingTemplate;
        public string ConfirmBookingTemplate { get => _confirmBookingTemplate; }
        public string CancelBookingTemplate { get => _cancelBookingTemplate; }
    }
}
