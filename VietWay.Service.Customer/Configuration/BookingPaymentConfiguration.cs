using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.Configuration
{
    public class BookingPaymentConfiguration(int paymentExpireAfterMinutes)
    {
        private readonly int _paymentExpireAfterMinutes = paymentExpireAfterMinutes;
        public int PendingPaymentExpireAfterMinutes { get => _paymentExpireAfterMinutes; }
    }
}
