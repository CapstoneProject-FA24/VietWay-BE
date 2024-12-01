using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class BookingPaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<BookingPayment?> GetBookingPaymentAsync(string id)
        {
            return await _unitOfWork.BookingPaymentRepository.Query()
                .Include(x => x.Booking)
                .SingleOrDefaultAsync(x => x.PaymentId.Equals(id));
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
                .Include(x => x.Booking)
                .SingleOrDefaultAsync(x => x.PaymentId.Equals(vnPayIPN.TxnRef) && x.Status == PaymentStatus.Pending);
            if (bookingPayment == null)
            {
                return;
            }
            bookingPayment.BankCode = vnPayIPN.BankCode;
            bookingPayment.BankTransactionNumber = vnPayIPN.BankTranNo;
            bookingPayment.PayTime = DateTime.ParseExact(vnPayIPN.PayDate, "yyyyMMddHHmmss", null);
            bookingPayment.ThirdPartyTransactionNumber = vnPayIPN.TransactionNo;
            if (vnPayIPN.TransactionStatus.Equals("00"))
            {
                int oldBookingStatus = (int)bookingPayment.Booking.Status;
                bookingPayment.Status = PaymentStatus.Paid;
                bookingPayment.Booking.Status = 
                    bookingPayment.Booking.PaidAmount + bookingPayment.Amount < bookingPayment.Booking.TotalPrice ?
                    BookingStatus.Deposited : BookingStatus.Paid;
                bookingPayment.Booking.PaidAmount += bookingPayment.Amount;
                string entityHistoryId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new()
                {
                    Action = EntityModifyAction.Update,
                    EntityType = EntityType.Booking,
                    EntityId = bookingPayment.BookingId,
                    Id = entityHistoryId,
                    ModifiedBy = bookingPayment.Booking.CustomerId,
                    ModifierRole = UserRole.Customer,
                    Reason = "Payment",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new()
                    {
                        Id = entityHistoryId,
                        NewStatus = (int)bookingPayment.Booking.Status,
                        OldStatus = oldBookingStatus
                    }
                });

            }
            else
            {
                bookingPayment.Status = PaymentStatus.Failed;
            }
            await _unitOfWork.BookingPaymentRepository.UpdateAsync(bookingPayment);
        }
    }
}
