using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;

namespace VietWay.Job.Implementation
{
    public class BookingPaymentJob(IRedisCacheService redisCacheService, IUnitOfWork unitOfWork) : IBookingPaymentJob
    {
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task CheckExpiredPayment(string paymentId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                BookingPayment? bookingPayment = await _unitOfWork
                    .BookingPaymentRepository
                    .Query()
                    .SingleOrDefaultAsync(x => x.PaymentId.Equals(paymentId) && x.Status == PaymentStatus.Pending);
                if (bookingPayment == null)
                {
                    return;
                }
                bookingPayment.Status = PaymentStatus.Failed;
                if (bookingPayment.ThirdPartyTransactionNumber != null)
                {
                    await _redisCacheService.RemoveAsync($"int32Id:{bookingPayment.ThirdPartyTransactionNumber}");
                }
                await _unitOfWork.BookingPaymentRepository.UpdateAsync(bookingPayment);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
