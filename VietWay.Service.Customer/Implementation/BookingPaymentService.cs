using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
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

        public async Task<PaginatedList<BookingPaymentDTO>> GetAllCustomerBookingPaymentsAsync(string customerId, int pageSize, int pageIndex)
        {
            IQueryable<BookingPayment> query = _unitOfWork.BookingPaymentRepository.Query()
                .Where(x => x.Booking!.CustomerId.Equals(customerId));
            int count = await query.CountAsync();
            List<BookingPaymentDTO> items = await query.OrderByDescending(x => x.CreateAt)
                .Select(x => new BookingPaymentDTO
                {
                    Amount = x.Amount,
                    BookingId = x.BookingId,
                    CreateAt = x.CreateAt,
                    PaymentId = x.PaymentId,
                    Status = x.Status,
                    BankCode = x.BankCode,
                    BankTransactionNumber = x.BankTransactionNumber,
                    Note = x.Note,
                    PayTime = x.PayTime,
                    ThirdPartyTransactionNumber = x.ThirdPartyTransactionNumber
                }).Skip((pageIndex-1)*pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<BookingPaymentDTO>
            {
                PageSize = pageSize,
                Total = count,
                PageIndex = pageIndex,
                Items = items
            };
        }

        public async Task<PaginatedList<BookingPaymentDTO>> GetBookingPaymentsAsync(string customerId, string bookingId, int pageSize, int pageIndex)
        {
            IQueryable<BookingPayment> query = _unitOfWork.BookingPaymentRepository.Query()
                .Where(x => x.BookingId.Equals(bookingId) && x.Booking!.CustomerId.Equals(customerId));
            int count = await query.CountAsync();
            List<BookingPaymentDTO> items = await query.OrderByDescending(x => x.CreateAt)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Select(x=> new BookingPaymentDTO
                {
                    Amount = x.Amount,
                    BookingId = x.BookingId,
                    CreateAt = x.CreateAt,
                    PaymentId = x.PaymentId,
                    Status = x.Status,
                    BankCode = x.BankCode,
                    BankTransactionNumber = x.BankTransactionNumber,
                    Note = x.Note,
                    PayTime = x.PayTime,
                    ThirdPartyTransactionNumber = x.ThirdPartyTransactionNumber
                })
                .ToListAsync();
            return new PaginatedList<BookingPaymentDTO>
            {
                Total = count,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = items
            };
        }

        public async Task<string> GetBookingPaymentUrl(PaymentMethod paymentMethod, bool isFullPayment, string bookingId, string customerId, string ipAddress)
        {
            Booking? tourBooking = await _unitOfWork.BookingRepository
                .Query()
                .Include(x => x.Tour)
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId) && x.CustomerId.Equals(customerId));
            if (tourBooking == null || tourBooking.Status != BookingStatus.Pending)
            {
                throw new ResourceNotFoundException("");
            }
            decimal amount;
            if (isFullPayment || tourBooking.Tour.DepositPercent == 0m)
            {
                amount = tourBooking.TotalPrice - tourBooking.PaidAmount;
            }
            else
            {
                amount = tourBooking.TotalPrice * tourBooking.Tour!.DepositPercent!.Value / 100;
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
