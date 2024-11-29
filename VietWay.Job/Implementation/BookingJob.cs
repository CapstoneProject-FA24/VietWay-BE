﻿using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;

namespace VietWay.Job.Implementation
{
    public class BookingJob(IUnitOfWork unitOfWork) : IBookingJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task CheckBookingForExpirationAsync(string bookingId)
        {
            Booking? booking = await _unitOfWork.BookingRepository
                .Query()
                .Include(x => x.Tour)
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId) && BookingStatus.Pending == x.Status);
            if (null == booking)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                booking.Status = BookingStatus.Cancelled;
#warning add history
                booking.Tour!.CurrentParticipant -= booking.NumberOfParticipants;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
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
