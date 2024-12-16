using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.Configuration
{
    public class BookingServiceConfiguration(int pendingBookingExpireAfterMinutes)
    {
        private readonly int _pendingBookingExpireAfterMinutes = pendingBookingExpireAfterMinutes;
        public int PendingBookingExpireAfterMinutes { get => _pendingBookingExpireAfterMinutes; }
    }
}
