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
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Email;

namespace VietWay.Job.Implementation
{
    public class EmailJob(EmailJobConfiguration configuration, IEmailService emailService, IUnitOfWork unitOfWork) : IEmailJob
    {
        private readonly EmailJobConfiguration _configuration = configuration;
        private readonly IEmailService _emailService = emailService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task SendBookingCancellationEmail(string bookingId)
        {
            var bookingDetail = await _unitOfWork.BookingRepository.Query()
                .Select(x => new
                {
                    x.Tour.TourTemplate.Code,
                    x.Tour.TourTemplate.TourName,
                    x.Tour.StartDate,
                    x.Tour.TourTemplate.TourDuration.NumberOfDay,
                    x.Tour.StartLocation,
                    x.BookingId,
                    x.TotalPrice,
                    x.CreatedAt,
                    x.Status,
                    x.ContactFullName,
                    x.ContactEmail,
                    x.ContactPhoneNumber,
                    x.ContactAddress,
                    NumberOfParticipants = x.BookingTourists.Count,
                    x.PaidAmount
                })
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId));
            if (bookingDetail == null)
            {
                return;
            }
            string template = _configuration.CancelBookingTemplate;
            string email = bookingDetail.ContactEmail!;
            template = template
                .Replace("{{{tourName}}}", bookingDetail.TourName)
                .Replace("{{{startDate}}}", bookingDetail.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{code}}}", bookingDetail.Code)
                .Replace("{{{startLocation}}}", bookingDetail.StartLocation)
                .Replace("{{{contactFullName}}}", bookingDetail.ContactFullName)
                .Replace("{{{contactEmail}}}", bookingDetail.ContactEmail)
                .Replace("{{{contactPhoneNumber}}}", bookingDetail.ContactPhoneNumber)
                .Replace("{{{contactAddress}}}", bookingDetail.ContactAddress)
                .Replace("{{{totalPrice}}}", bookingDetail.TotalPrice.ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{createdAt}}}", bookingDetail.CreatedAt.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{numberOfParticipants}}}", bookingDetail.NumberOfParticipants.ToString())
                .Replace("{{{bookingId}}}", bookingDetail.BookingId)
                .Replace("{{{status}}}", bookingDetail.Status.ToString())
                .Replace("{{{paidAmount}}}",bookingDetail.PaidAmount.ToString(@"C", new CultureInfo("vi-VN")));
            await _emailService.SendEmailAsync(email, "Hủy booking", template);
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

        public async Task SendBookingPaymentExpiredEmail(string bookingId)
        {
            var bookingDetail = await _unitOfWork.BookingRepository.Query()
                .Select(x => new
                {
                    x.Tour.TourTemplate.Code,
                    x.Tour.TourTemplate.TourName,
                    x.Tour.StartDate,
                    x.Tour.TourTemplate.TourDuration.NumberOfDay,
                    x.Tour.StartLocation,
                    x.BookingId,
                    x.TotalPrice,
                    x.CreatedAt,
                    x.Status,
                    x.ContactFullName,
                    x.ContactEmail,
                    x.ContactPhoneNumber,
                    x.ContactAddress,
                    NumberOfParticipants = x.BookingTourists.Count,
                    x.PaidAmount
                })
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId));
            if (bookingDetail == null)
            {
                return;
            }
            string template = _configuration.BookingExpiredTemplate;
            string email = bookingDetail.ContactEmail!;
            template = template
                .Replace("{{{tourName}}}", bookingDetail.TourName)
                .Replace("{{{startDate}}}", bookingDetail.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{code}}}", bookingDetail.Code)
                .Replace("{{{startLocation}}}", bookingDetail.StartLocation)
                .Replace("{{{contactFullName}}}", bookingDetail.ContactFullName)
                .Replace("{{{contactEmail}}}", bookingDetail.ContactEmail)
                .Replace("{{{contactPhoneNumber}}}", bookingDetail.ContactPhoneNumber)
                .Replace("{{{contactAddress}}}", bookingDetail.ContactAddress)
                .Replace("{{{totalPrice}}}", bookingDetail.TotalPrice.ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{createdAt}}}", bookingDetail.CreatedAt.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{numberOfParticipants}}}", bookingDetail.NumberOfParticipants.ToString())
                .Replace("{{{bookingId}}}", bookingDetail.BookingId)
                .Replace("{{{status}}}", bookingDetail.Status.ToString())
                .Replace("{{{paidAmount}}}", bookingDetail.PaidAmount.ToString(@"C", new CultureInfo("vi-VN")));
            await _emailService.SendEmailAsync(email, "Hủy booking", template);
        }

        public async Task SendBookingTourChangeEmail(string bookingId,string oldTourId, string newTourId)
        {
            Tour? oldTour = _unitOfWork.TourRepository.Query()
                .Include(x => x.TourTemplate)
                .SingleOrDefault(x => x.TourId.Equals(oldTourId));
            Tour? newTour = _unitOfWork.TourRepository.Query()
                .Include(x => x.TourTemplate)
                .SingleOrDefault(x => x.TourId.Equals(newTourId));
            if (oldTour == null || newTour == null)
            {
                return;
            }
            var bookingDetail = await _unitOfWork.BookingRepository.Query()
                .Select(x => new
                {
                    x.Tour.TourTemplate.Code,
                    x.Tour.TourTemplate.TourName,
                    x.Tour.StartDate,
                    x.Tour.TourTemplate.TourDuration.NumberOfDay,
                    x.Tour.StartLocation,
                    x.BookingId,
                    x.TotalPrice,
                    x.CreatedAt,
                    x.Status,
                    x.ContactFullName,
                    x.ContactEmail,
                    x.ContactPhoneNumber,
                    x.ContactAddress,
                    NumberOfParticipants = x.BookingTourists.Count,
                    x.PaidAmount
                })
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId));
            if (bookingDetail == null)
            {
                return;
            }
            string template = _configuration.BookingChangedTemplate;
            template = template.Replace("{{{oldTourCode", oldTour.TourTemplate.Code)
                .Replace("{{{oldTourName}}", oldTour.TourTemplate.TourName)
                .Replace("{{{oldStartDate}}", oldTour.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{oldStartLocation}}", oldTour.StartLocation)
                .Replace("{{{newTourCode}}", newTour.TourTemplate.Code)
                .Replace("{{{newTourName}}", newTour.TourTemplate.TourName)
                .Replace("{{{newStartDate}}", newTour.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{newStartLocation}}", newTour.StartLocation)
                .Replace("{{{contactFullName}}", bookingDetail.ContactFullName)
                .Replace("{{{contactEmail}}", bookingDetail.ContactEmail)
                .Replace("{{{contactPhoneNumber}}", bookingDetail.ContactPhoneNumber)
                .Replace("{{{contactAddress}}", bookingDetail.ContactAddress)
                .Replace("{{{bookingId}}}",bookingDetail.BookingId)
                .Replace("{{{totalPrice}}", bookingDetail.TotalPrice.ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{createdAt}}", bookingDetail.CreatedAt.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{numberOfParticipants}}", bookingDetail.NumberOfParticipants.ToString())
                .Replace("{{{status}}", bookingDetail.Status.ToString())
                .Replace("{{{remainingAmount}}}",(bookingDetail.TotalPrice-bookingDetail.PaidAmount > 0 ? bookingDetail.TotalPrice - bookingDetail.PaidAmount : 0).ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{paidAmount}}", bookingDetail.PaidAmount.ToString(@"C", new CultureInfo("vi-VN")));
                
            await _emailService.SendEmailAsync(bookingDetail.ContactEmail, "Thay đổi tour", template);
        }

        public Task SendNewPasswordEmail(string email, string name, string newPassword)
        {
            string template = _configuration.ResetPasswordTemplate;
            template = template.Replace("{{{fullName}}}", name)
                .Replace("{{{newPassword}}}", newPassword);
            return _emailService.SendEmailAsync(email, "Mật khẩu mới", template);
        }

        public async Task SendSystemCancellationEmail(string bookingId, string? reason)
        {
            var bookingDetail = await _unitOfWork.BookingRepository.Query()
                .Select(x => new {
                    x.Tour.TourTemplate.TourName,
                    x.Tour.StartDate,
                    x.Tour.TourTemplate.Code,
                    x.Tour.StartLocation,
                    x.ContactAddress,
                    x.ContactEmail,
                    x.ContactFullName,
                    x.ContactPhoneNumber,
                    x.BookingId,
                    x.TotalPrice,
                    x.CreatedAt,
                    x.Status,
                    NumberOfParticipants = x.BookingTourists.Count,
                    x.PaidAmount
                }).SingleOrDefaultAsync();
            if (bookingDetail == null)
            {
                return;
            }
            string template = _configuration.VietwayCancelBookingTemplate;
            template = template
                .Replace("{{{tourName}}}", bookingDetail.TourName)
                .Replace("{{{startDate}}}", bookingDetail.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{reason}}}", reason??"")
                .Replace("{{{code}}}", bookingDetail.Code)
                .Replace("{{{startLocation}}}", bookingDetail.StartLocation)
                .Replace("{{{contactFullName}}}", bookingDetail.ContactFullName)
                .Replace("{{{contactEmail}}}", bookingDetail.ContactEmail)
                .Replace("{{{contactPhoneNumber}}}", bookingDetail.ContactPhoneNumber)
                .Replace("{{{contactAddress}}}", bookingDetail.ContactAddress)
                .Replace("{{{totalPrice}}}", bookingDetail.TotalPrice.ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{paidAmount}}}", bookingDetail.PaidAmount.ToString(@"C", new CultureInfo("vi-VN")))
                .Replace("{{{createAt}}}", bookingDetail.CreatedAt.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{numberOfParticipants}}}", bookingDetail.NumberOfParticipants.ToString())
                .Replace("{{{bookingId}}}", bookingDetail.BookingId)
                .Replace("{{{status}}}", bookingDetail.Status.ToString());
            await _emailService.SendEmailAsync(bookingDetail.ContactEmail, "Hủy booking", template);
        }

        public async Task SendWarningMailClosedTourNotEnoughParticipantManager(string tourId)
        {
            List<string> managerEmails = await _unitOfWork.AccountRepository.Query()
                .Where(x => x.Role == UserRole.Manager && x.Email!=null && x.IsDeleted == false)
                .Select(x => x.Email)
                .ToListAsync();
            if (managerEmails.Count == 0)
            {
                return;
            }
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .Include(x => x.TourTemplate)
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId));
            if (tour == null)
            {
                return;
            }
            string template = _configuration.WarningClosedTourNotEnoughParticipantTemplate;
            template = template.Replace("{{{code}}}",tourId)
                .Replace("{{{tourName}}}", tour.TourTemplate.TourName)
                .Replace("{{{endDate}}}", tour.RegisterCloseDate.Value.ToString(@"F", new CultureInfo("vi-VN")))
                .Replace("{{{currentParticipants}}}", tour.CurrentParticipant.ToString())
                .Replace("{{{minParticipants}}}", tour.MinParticipant.ToString())
                .Replace("{{{startDate}}}",tour.StartDate.Value.ToString(@"F", new CultureInfo("vi-VN")));
            foreach (var email in managerEmails)
            {
                await _emailService.SendEmailAsync(email, "Tour không đủ người tham gia", template);
            }
        }
    }
}
