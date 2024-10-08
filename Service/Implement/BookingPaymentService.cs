using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.IdHelper;

namespace VietWay.Service.Implement
{
    public class BookingPaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService, IIdGenerator idGenerator) : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<BookingPayment?> GetBookingPaymentAsync(string id)
        {
            return await _unitOfWork.BookingPaymentRepository.Query()
                .Include(x => x.TourBooking)
                .SingleOrDefaultAsync(x=>x.PaymentId.Equals(id));
        }

        public async Task<string> GetVnPayBookingPaymentUrl(string bookingId, string ipAddress)
        {
            TourBooking? tourBooking = await _unitOfWork.TourBookingRepository
                .Query()
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId));
            if (tourBooking == null || tourBooking.Status != BookingStatus.Pending)
            {
                throw new Exception();
            }
            BookingPayment bookingPayment = new()
            {
                PaymentId = _idGenerator.GenerateId(),
                Amount = tourBooking.TotalPrice,
                Status = PaymentStatus.Pending,
                BookingId = bookingId,
                CreateOn = DateTime.UtcNow,
            };
            await _unitOfWork.BookingPaymentRepository.Create(bookingPayment);
            return _vnPayService.GetPaymentUrl(bookingPayment, ipAddress);
        }

        public async Task HandleVnPayIPN(VnPayIPN vnPayIPN)
        {
            if (_vnPayService.VerifyTransaction(vnPayIPN) == false)
            {
                return;
            }
            BookingPayment? bookingPayment = await _unitOfWork
                .BookingPaymentRepository
                .Query()
                .Include(x => x.TourBooking)
                .SingleOrDefaultAsync(x => x.PaymentId.Equals(vnPayIPN.TxnRef) && x.Status == PaymentStatus.Pending );
            if (bookingPayment == null)
            {
                return;
            }
            bookingPayment.BankCode = vnPayIPN.BankCode;
            bookingPayment.BankTransactionNumber = vnPayIPN.BankTranNo;
            bookingPayment.PayTime = DateTime.Parse(vnPayIPN.PayDate);
            bookingPayment.ThirdPartyTransactionNumber = vnPayIPN.TransactionNo;
            if (vnPayIPN.TransactionStatus.Equals("00"))
            {
                bookingPayment.Status = PaymentStatus.Paid;
                bookingPayment.TourBooking.Status = BookingStatus.Confirmed;
            }
            else
            {
                bookingPayment.Status = PaymentStatus.Failed;
            }
            await _unitOfWork.BookingPaymentRepository.Update(bookingPayment);
        }
    }
}
