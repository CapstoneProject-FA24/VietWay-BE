using Hangfire;
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
using VietWay.Util.DateTimeUtil;

namespace VietWay.Job.Implementation
{
    public class TourJob(IUnitOfWork unitOfWork, IBackgroundJobClient backgroundJobClient, ITimeZoneHelper timeZoneHelper) : ITourJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
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
                _backgroundJobClient.Schedule<ITourJob>(x => x.CloseTourAsync(tourId),
                    _timeZoneHelper.GetLocalTimeFromUTC7(tour.RegisterCloseDate!.Value));
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
                _backgroundJobClient.Schedule<ITourJob>(x => x.ChangeTourToOngoingAsync(tourId),
                    _timeZoneHelper.GetLocalTimeFromUTC7(tour.StartDate!.Value));
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
                    .Include(x=>x.TourTemplate.TourDuration)
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
                _backgroundJobClient.Schedule<ITourJob>(x => x.ChangeTourToCompletedAsync(tourId),
                    _timeZoneHelper.GetLocalTimeFromUTC7(tour.StartDate!.Value.AddDays(tour.TourTemplate.TourDuration.NumberOfDay)));
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
