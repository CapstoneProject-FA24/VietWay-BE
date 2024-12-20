using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty.ZaloPay;

namespace VietWay.Job.Interface
{
    public interface IEmailJob
    {
        public Task SendBookingConfirmationEmail(string bookingId, DateTime paymentDeadline);
        public Task SendBookingCancellationEmail(string bookingId);
        public Task SendSystemCancellationEmail(string bookingId, string? reaon);
        public Task SendBookingTourChangeEmail(string bookingId, string oldTourId, string newTourId);
        public Task SendBookingPaymentExpiredEmail(string bookingId);
        public Task SendNewPasswordEmail(string email, string name, string newPassword);
    }
}
