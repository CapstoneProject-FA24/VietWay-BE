using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Job.Customer.Interface;
using VietWay.Repository.EntityModel;

namespace VietWay.Job.Customer.Implementation
{
    public class EmailJob : IEmailJob
    {
        public Task SendBookingCancellationEmail(Booking booking)
        {
            throw new NotImplementedException();
        }

        public Task SendBookingConfirmationEmail(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
