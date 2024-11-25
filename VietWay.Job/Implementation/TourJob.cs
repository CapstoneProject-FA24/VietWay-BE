using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;

namespace VietWay.Job.Implementation
{
    public class TourJob(IUnitOfWork unitOfWork) : ITourJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task OpenTourAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.TourId.Equals(tourId) && x.Status == TourStatus.Accepted)
                    .SingleOrDefaultAsync();
            if (null == tour)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.Status = TourStatus.Opened;
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task CloseTourAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.TourId.Equals(tourId) && x.Status == TourStatus.Opened)
                    .SingleOrDefaultAsync();
            if (null == tour)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.Status = TourStatus.Closed;
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task ChangeTourToOngoingAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.TourId.Equals(tourId) && x.Status == TourStatus.Closed)
                    .SingleOrDefaultAsync();
            if (null == tour)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.Status = TourStatus.OnGoing;
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task ChangeTourToCompletedAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.TourId.Equals(tourId) && x.Status == TourStatus.OnGoing)
                    .SingleOrDefaultAsync();
            if (null == tour)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.Status = TourStatus.Completed;
                await _unitOfWork.TourRepository.UpdateAsync(tour);
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
