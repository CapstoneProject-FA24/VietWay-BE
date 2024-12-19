using Hangfire;
using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Configuration;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.PayOS;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Service.ThirdParty.ZaloPay;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;
namespace VietWay.Service.Customer.Implementation
{
    public class BookingPaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService, IIdGenerator idGenerator, 
        IZaloPayService zaloPayService, ITimeZoneHelper timeZoneHelper, IPayOSService payOSService,
        IRedisCacheService redisCacheService, BookingPaymentConfiguration configuration, 
        IBackgroundJobClient backgroundJobClient) : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IZaloPayService _zaloPayService = zaloPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IPayOSService _payOSService = payOSService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly int _paymentExpireAfterMinutes = configuration.PendingPaymentExpireAfterMinutes;

        public async Task<PaginatedList<BookingPaymentDTO>> GetAllCustomerBookingPaymentsAsync(string customerId, int pageSize, int pageIndex)
        {
            IQueryable<BookingPayment> query = _unitOfWork.BookingPaymentRepository.Query()
                .Where(x => x.Booking!.CustomerId.Equals(customerId));
            int count = await query.CountAsync();
            List<BookingPaymentDTO> items = await query.OrderByDescending(x => x.CreatedAt)
                .Select(x => new BookingPaymentDTO
                {
                    Amount = x.Amount,
                    BookingId = x.BookingId,
                    CreateAt = x.CreatedAt,
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
            List<BookingPaymentDTO> items = await query.OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Select(x=> new BookingPaymentDTO
                {
                    Amount = x.Amount,
                    BookingId = x.BookingId,
                    CreateAt = x.CreatedAt,
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

        public async Task<string> GetBookingPaymentUrl(PaymentMethod paymentMethod, bool? isFullPayment, string bookingId, string customerId, string ipAddress)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Booking? tourBooking = await _unitOfWork.BookingRepository
                    .Query()
                    .Include(x => x.Tour.TourTemplate)
                    .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId) && x.CustomerId.Equals(customerId));
                if (tourBooking == null || (tourBooking.Status != BookingStatus.Pending && tourBooking.Status != BookingStatus.Deposited))
                {
                    throw new ResourceNotFoundException("NOT_EXIST_BOOKING");
                }
                decimal amount;
                if (isFullPayment == null || isFullPayment.Value || tourBooking.Tour.DepositPercent == 0m)
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
                    Amount = amount,
                    Status = PaymentStatus.Pending,
                    BookingId = bookingId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                if (paymentMethod == PaymentMethod.PayOS)
                {
                    bookingPayment.ThirdPartyTransactionNumber = (await _redisCacheService.CreateIntId(bookingPayment.PaymentId)).ToString();
                }
                await _unitOfWork.BookingPaymentRepository.CreateAsync(bookingPayment);
                await _unitOfWork.CommitTransactionAsync();
                string paymentUrl = paymentMethod switch
                {
                    PaymentMethod.VNPay => _vnPayService.GetPaymentUrl(bookingPayment, ipAddress, _paymentExpireAfterMinutes),
                    PaymentMethod.ZaloPay => await _zaloPayService.GetPaymentUrl(bookingPayment, _paymentExpireAfterMinutes),
                    PaymentMethod.PayOS => await _payOSService.CreatePaymentUrl(
                        bookingPayment,
                        tourBooking.Tour!.TourTemplate!.TourName!,
                        _paymentExpireAfterMinutes),
                    _ => throw new InvalidInfoException("INVALID_INFO_PAYMENT_METHOD")
                };
                _backgroundJobClient.Schedule<IBookingPaymentJob>(x => x.CheckExpiredPayment(bookingPayment.PaymentId),TimeSpan.FromMinutes(_paymentExpireAfterMinutes));
                return paymentUrl;
            } catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
