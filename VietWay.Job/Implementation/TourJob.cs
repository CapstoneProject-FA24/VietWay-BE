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
using VietWay.Util.IdUtil;

namespace VietWay.Job.Implementation
{
    public class TourJob(IUnitOfWork unitOfWork, IBackgroundJobClient backgroundJobClient, 
        ITimeZoneHelper timeZoneHelper, IIdGenerator idGenerator) : ITourJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
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
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Admin,
                    ModifiedBy = null,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory {
                        Id = historyId,
                        OldStatus = (int)TourStatus.Accepted,
                        NewStatus = (int)TourStatus.Opened,
                    }
                });
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
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Admin,
                    ModifiedBy = null,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = historyId,
                        OldStatus = (int)TourStatus.Opened,
                        NewStatus = (int)TourStatus.Closed,
                    }
                });
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
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Admin,
                    ModifiedBy = null,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = historyId,
                        OldStatus = (int)TourStatus.Closed,
                        NewStatus = (int)TourStatus.OnGoing,
                    }
                });
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
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Admin,
                    ModifiedBy = null,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = historyId,
                        OldStatus = (int)TourStatus.OnGoing,
                        NewStatus = (int)TourStatus.Completed,
                    }
                });
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task RejectUnapprovedTourAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.TourId.Equals(tourId) && x.Status == TourStatus.Pending)
                    .SingleOrDefaultAsync();
            if (null == tour)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.Status = TourStatus.Rejected;
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Admin,
                    ModifiedBy = null,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = historyId,
                        OldStatus = (int)TourStatus.Pending,
                        NewStatus = (int)TourStatus.Rejected,
                    }
                });
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
