using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;
namespace VietWay.Service.Customer.Implementation
{
    public class BookingPaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService, IIdGenerator idGenerator,
        ITimeZoneHelper timeZoneHelper) : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<string> GetBookingPaymentUrl(PaymentMethod paymentMethod, string bookingId, string customerId, string ipAddress)
        {
            Booking? tourBooking = await _unitOfWork.BookingRepository
                .Query()
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId) && x.CustomerId.Equals(customerId));
            if (tourBooking == null || tourBooking.Status != BookingStatus.Pending)
            {
                throw new ResourceNotFoundException("");
            }
            BookingPayment bookingPayment = new()
            {
                PaymentId = _idGenerator.GenerateId(),
                Amount = tourBooking.TotalPrice,
                Status = PaymentStatus.Pending,
                BookingId = bookingId,
                CreateAt = _timeZoneHelper.GetUTC7Now(),
            };
            await _unitOfWork.BookingPaymentRepository.CreateAsync(bookingPayment);
            return _vnPayService.GetPaymentUrl(bookingPayment, ipAddress);
        }
    }
}
