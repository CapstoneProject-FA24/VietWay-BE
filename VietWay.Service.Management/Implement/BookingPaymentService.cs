using Azure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
using VietWay.Service.ThirdParty.ZaloPay;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class BookingPaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, IZaloPayService zaloPayService) : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IZaloPayService _zaloPayService = zaloPayService;
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

        public async Task HandleZaloPayCallbackLocal(ZaloPayCallback zaloPayCallback)
        {
            ZaloPayResponse response = await _zaloPayService.VerifyTransactionLocal(zaloPayCallback);
            if (response.ReturnCode == 2)
            {
                return;
            }

            string[] parts = zaloPayCallback.AppTransId.Split('_');
            string data = parts.Length > 1 ? parts[1] : string.Empty;

            BookingPayment? bookingPayment = await _unitOfWork
                .BookingPaymentRepository
                .Query()
                .Include(x => x.Booking)
                .SingleOrDefaultAsync(x => x.PaymentId.Equals(data) && x.Status == PaymentStatus.Pending);
            if (bookingPayment == null)
            {
                return;
            }

            bookingPayment.BankCode = "ZaloPayApp";
            bookingPayment.BankTransactionNumber = string.Empty;

            DateTime utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(response.ServerTime).UtcDateTime;
            TimeZoneInfo utcPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime utcPlus7DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, utcPlus7);
            bookingPayment.PayTime = utcPlus7DateTime;

            bookingPayment.ThirdPartyTransactionNumber = response.ZpTransId.ToString();
            if (response.ReturnCode.Equals(1))
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

        public async Task<Dictionary<string, object>> HandleZaloPayCallback(CallbackData data)
        {
            var dataJson = JsonConvert.DeserializeObject<CallbackDataContent>(data.Data);
            Dictionary<string, object> result = _zaloPayService.VerifyTransaction(data);
            string[] parts = dataJson.AppTransId.Split('_');
            string appTransId = parts.Length > 1 ? parts[1] : string.Empty;

            BookingPayment? bookingPayment = await _unitOfWork
                .BookingPaymentRepository
                .Query()
                .Include(x => x.Booking)
                .SingleOrDefaultAsync(x => x.PaymentId.Equals(appTransId) && x.Status == PaymentStatus.Pending);
            if (bookingPayment == null)
            {
                return result;
            }

            bookingPayment.BankCode = "ZaloPayApp";
            bookingPayment.BankTransactionNumber = string.Empty;

            DateTime utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(dataJson.ServerTime).UtcDateTime;
            TimeZoneInfo utcPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime utcPlus7DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, utcPlus7);
            bookingPayment.PayTime = utcPlus7DateTime;

            bookingPayment.ThirdPartyTransactionNumber = dataJson.ZpTransId.ToString();
            if (result["return_code"].Equals(1))
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
            return result;
        }
    }
}
