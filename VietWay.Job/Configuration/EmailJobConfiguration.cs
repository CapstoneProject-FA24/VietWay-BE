using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Job.Configuration
{
    public class EmailJobConfiguration(string cancelBookingTemplate, string confirmBookingTemplate, string vietwayCancelBookingTemplate, string resetPasswordTemplate)
    {
        private string _cancelBookingTemplate = cancelBookingTemplate;
        private string _confirmBookingTemplate = confirmBookingTemplate;
        private string _vietwayCancelBookingTemplate = vietwayCancelBookingTemplate;
        private string _resetPasswordTemplate = resetPasswordTemplate;
        public string ConfirmBookingTemplate { get => _confirmBookingTemplate; }
        public string CancelBookingTemplate { get => _cancelBookingTemplate; }
        public string VietwayCancelBookingTemplate { get => _vietwayCancelBookingTemplate; }
        public string ResetPasswordTemplate { get => _resetPasswordTemplate; }
    }
}
