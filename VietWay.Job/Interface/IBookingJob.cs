using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Job.Customer.Interface
{
    public interface IBookingJob
    {
        public Task CheckBookingForExpirationJob(string bookingId);
    }
}
