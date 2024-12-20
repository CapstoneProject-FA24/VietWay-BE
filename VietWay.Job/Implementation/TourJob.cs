using Hangfire;
using Microsoft.EntityFrameworkCore;
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
        public async Task OpenToursAsync()
        {
            List<Tour> tours = await _unitOfWork.TourRepository.Query()
                    .Where(x=> x.Status == TourStatus.Accepted && x.RegisterOpenDate > _timeZoneHelper.GetUTC7Now())
                    .ToListAsync();
            foreach (Tour tour in tours)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    tour.Status = TourStatus.Opened;
                    await _unitOfWork.TourRepository.UpdateAsync(tour);
                    string historyId = _idGenerator.GenerateId();
                    await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                    {
                        EntityId = tour.TourId,
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
                            OldStatus = (int)TourStatus.Accepted,
                            NewStatus = (int)TourStatus.Opened,
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
        public async Task CloseToursAsync()
        {
            List<Tour> tours = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.Status == TourStatus.Opened && x.RegisterCloseDate > _timeZoneHelper.GetUTC7Now())
                    .ToListAsync();
            foreach (Tour tour in tours)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    tour.Status = TourStatus.Closed;
                    await _unitOfWork.TourRepository.UpdateAsync(tour);
                    string historyId = _idGenerator.GenerateId();
                    await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                    {
                        EntityId = tour.TourId,
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
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }
        public async Task ChangeToursToOngoingAsync()
        {
            List<Tour> tours = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.Status == TourStatus.Closed && x.StartDate > _timeZoneHelper.GetUTC7Now())
                    .ToListAsync();
            foreach (Tour tour in tours)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    tour.Status = TourStatus.OnGoing;
                    await _unitOfWork.TourRepository.UpdateAsync(tour);
                    string historyId = _idGenerator.GenerateId();
                    await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                    {
                        EntityId = tour.TourId,
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
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }
        public async Task ChangeToursToCompletedAsync()
        {
            List<Tour> tours = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.Status == TourStatus.OnGoing && x.StartDate.Value.AddDays(x.TourTemplate.TourDuration.NumberOfDay) > _timeZoneHelper.GetUTC7Now())
                    .ToListAsync();
            foreach (Tour tour in tours)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    tour.Status = TourStatus.Completed;
                    await _unitOfWork.TourRepository.UpdateAsync(tour);
                    string historyId = _idGenerator.GenerateId();
                    await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                    {
                        EntityId = tour.TourId,
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
        }
        public async Task RejectUnapprovedToursAsync()
        {
            List<Tour> tours = await _unitOfWork.TourRepository.Query()
                    .Where(x => x.Status == TourStatus.Pending && x.RegisterCloseDate > _timeZoneHelper.GetUTC7Now())
                    .ToListAsync();
            foreach (Tour tour in tours)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    tour.Status = TourStatus.Rejected;
                    await _unitOfWork.TourRepository.UpdateAsync(tour);
                    string historyId = _idGenerator.GenerateId();
                    await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                    {
                        EntityId = tour.TourId,
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
}
