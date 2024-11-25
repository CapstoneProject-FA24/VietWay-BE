using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Job.Configuration;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Email;

namespace VietWay.Job.Implementation
{
    public class EmailJob(EmailJobConfiguration configuration, IEmailService emailService, IUnitOfWork unitOfWork) : IEmailJob
    {
        private readonly EmailJobConfiguration _configuration = configuration;
        private readonly IEmailService _emailService = emailService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public Task SendBookingCancellationEmail(Booking booking)
        {
            throw new NotImplementedException();
        }

        public async Task SendBookingConfirmationEmail(string bookingId, DateTime paymentDeadline)
        {
            var bookingDetail = await _unitOfWork.BookingRepository.Query()
                .Select(x => new
                {
                    x.Tour.TourTemplate.Code,
                    x.Tour.TourTemplate.TourName,
                    TourSchedule = x.Tour.TourTemplate.TourTemplateSchedules.OrderBy(x=>x.DayNumber).ToList(),
                    x.Tour.StartDate,
                    x.Tour.TourTemplate.TourDuration.NumberOfDay,
                    x.Tour.StartLocation,
                    x.BookingId,
                    x.TotalPrice,
                    BookingDate = x.CreatedAt,
                    x.Status,
                    x.ContactFullName,
                    x.ContactEmail,
                    x.ContactPhoneNumber,
                    x.ContactAddress,
                    TotalPassenger = x.BookingTourists.Count
                })
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId));

            if (bookingDetail == null)
            {
                return;
            }
            string scheduleDetail = "";
            foreach (var schedule in bookingDetail.TourSchedule)
            {
                scheduleDetail += $"Ngày {schedule.DayNumber}: {schedule.Title}<br>{schedule.Description}<br>";
            }

            string template = _configuration.ConfirmBookingTemplate;
            string email = bookingDetail.ContactEmail!;
            template = template.Replace("{{{tourCode}}}", bookingDetail.Code);
            template = template.Replace("{{{tourName}}}", bookingDetail.TourName);
            template = template.Replace("{{{tourSchedule}}}", scheduleDetail);
            template = template.Replace("{{{startDate}}}", bookingDetail.StartDate.Value.ToString(@"F",new CultureInfo("vi-VN")));
            template = template.Replace("{{{endDate}}}", bookingDetail.StartDate.Value.AddDays(bookingDetail.NumberOfDay).ToString(@"F", new CultureInfo("vi-VN")));
            template = template.Replace("{{{startLocation}}}", bookingDetail.StartLocation);
            template = template.Replace("{{{bookingId}}}", bookingDetail.BookingId);
            template = template.Replace("{{{totalPrice}}}", bookingDetail.TotalPrice.ToString(@"C", new CultureInfo("vi-VN")));
            template = template.Replace("{{{bookingDate}}}", bookingDetail.BookingDate.ToString(@"F", new CultureInfo("vi-VN")));
            template = template.Replace("{{{paymentDeadline}}}", paymentDeadline.ToString(@"F", new CultureInfo("vi-VN")));
            template = template.Replace("{{{customerName}}}", bookingDetail.ContactFullName);
            template = template.Replace("{{{customerEmail}}}", bookingDetail.ContactEmail);
            template = template.Replace("{{{customerPhone}}}", bookingDetail.ContactPhoneNumber);
            template = template.Replace("{{{customerAddress}}}", bookingDetail.ContactAddress);
            template = template.Replace("{{{totalPassengers}}}", bookingDetail.TotalPassenger.ToString());

            await _emailService.SendEmailAsync(email, "Xác nhận booking", template);

        }
    }
}
