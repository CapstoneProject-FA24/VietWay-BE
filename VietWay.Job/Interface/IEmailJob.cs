using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Job.Interface
{
    public interface IEmailJob
    {
        public Task SendBookingConfirmationEmail(string bookingId, DateTime paymentDeadline);
        public Task SendBookingCancellationEmail(Booking booking);
    }
}
