using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Job.Customer.Interface
{
    public interface IEmailJob
    {
        public Task SendBookingConfirmationEmail(Booking booking);
        public Task SendBookingCancellationEmail(Booking booking);
    }
}
