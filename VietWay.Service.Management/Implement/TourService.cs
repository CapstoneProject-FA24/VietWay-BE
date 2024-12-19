using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.Interface;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Util.CustomExceptions;
using Hangfire;
using VietWay.Job.Interface;
using Tweetinvi.Core.Extensions;

namespace VietWay.Service.Management.Implement
{
    public class TourService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, IBackgroundJobClient backgroundJobClient) : ITourService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        public async Task<string> CreateTour(Tour tour, string staffId)
        {
            try
            {
                bool isStaff = await _unitOfWork.AccountRepository.Query()
                    .AnyAsync(x => x.AccountId.Equals(staffId) && x.Role == UserRole.Staff && x.IsDeleted == false);
                if (!isStaff)
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }
                await _unitOfWork.BeginTransactionAsync();
                tour.CreatedAt = _timeZoneHelper.GetUTC7Now();
                tour.TourId = _idGenerator.GenerateId();
                foreach (TourPrice item in tour.TourPrices)
                {
                    item.PriceId = _idGenerator.GenerateId();
                    item.TourId = tour.TourId;
                }
                foreach (TourRefundPolicy item in tour.TourRefundPolicies)
                {
                    item.TourRefundPolicyId = _idGenerator.GenerateId();
                    item.TourId = tour.TourId;
                }
                await _unitOfWork.TourRepository
                    .CreateAsync(tour);
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    Action = EntityModifyAction.Create,
                    EntityId = tour.TourId,
                    EntityType = EntityType.Tour,
                    Id = _idGenerator.GenerateId(),
                    ModifiedBy = staffId,
                    ModifierRole = UserRole.Staff,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now()
                });
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Schedule<ITourJob>(x => x.RejectUnapprovedTourAsync(tour.TourId), _timeZoneHelper.GetLocalTimeFromUTC7(tour.RegisterCloseDate!.Value));
                return tour.TourId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task EditTour(string tourId, Tour updatedTour, string accountId)
        {
            UserRole role = await _unitOfWork.AccountRepository.Query()
                .Where(x => x.AccountId.Equals(accountId))
                .Select(x => x.Role)
                .SingleOrDefaultAsync();
            if (role != UserRole.Manager && role != UserRole.Staff)
            {
                throw new UnauthorizedException("UNAUTHORIZED");
            }
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .Include(x => x.TourPrices)
                .Include(x => x.TourRefundPolicies)
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_TOUR");
            switch (tour.Status)
            {
                case TourStatus.Opened when tour.CurrentParticipant != 0:
                    throw new InvalidActionException("INVALID_ACTION_EDIT_TOUR_HAS_BOOKING");
                case TourStatus.Closed:
                    throw new InvalidActionException("INVALID_ACTION_EDIT_CLOSED_TOUR");
                case TourStatus.Completed:
                    throw new InvalidActionException("INVALID_ACTION_EDIT_COMPLETED_TOUR");
                case TourStatus.Cancelled:
                    throw new InvalidActionException("INVALID_ACTION_EDIT_CANCELLED_TOUR");
                default:
                    break;
            }
            tour.StartLocation = updatedTour.StartLocation;
            tour.StartDate = updatedTour.StartDate;
            tour.DefaultTouristPrice = updatedTour.DefaultTouristPrice;
            tour.RegisterOpenDate = updatedTour.RegisterOpenDate;
            tour.RegisterCloseDate = updatedTour.RegisterCloseDate;
            tour.MinParticipant = updatedTour.MinParticipant;
            tour.MaxParticipant = updatedTour.MaxParticipant;
            tour.Status = TourStatus.Pending;
            tour.DepositPercent = updatedTour.DepositPercent;
            tour.PaymentDeadline = updatedTour.PaymentDeadline;

            if (updatedTour.TourPrices != null)
            {
                tour.TourPrices.Clear();

                foreach (TourPrice priceInfo in updatedTour.TourPrices)
                {
                    tour.TourPrices.Add(new TourPrice
                    {
                        PriceId = _idGenerator.GenerateId(),
                        Name = priceInfo.Name,
                        Price = priceInfo.Price,
                        AgeFrom = priceInfo.AgeFrom,
                        AgeTo = priceInfo.AgeTo,
                        TourId = tourId 
                    });
                }
            }

            if (updatedTour.TourRefundPolicies != null)
            {
                tour.TourRefundPolicies.Clear();

                foreach (TourRefundPolicy refundPolicy in updatedTour.TourRefundPolicies)
                {
                    tour.TourRefundPolicies.Add(new TourRefundPolicy
                    {
                        TourRefundPolicyId = _idGenerator.GenerateId(),
                        CancelBefore = refundPolicy.CancelBefore,
                        RefundPercent = refundPolicy.RefundPercent,
                        TourId = tourId
                    });
                }
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.Update,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = role,
                    ModifiedBy = accountId,
                    Reason = "",
                    Timestamp = _timeZoneHelper.GetUTC7Now()
                });
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<(int totalCount, List<TourPreviewDTO> items)> GetAllTour(string? nameSearch, string? codeSearch, List<string>? provinceIds, List<string>? tourCategoryIds, List<string>? durationIds, TourStatus? status, int pageSize, int pageIndex, DateTime? startDateFrom, DateTime? startDateTo)
        {
            IQueryable<Tour> query = _unitOfWork
                .TourRepository
                .Query()
                .Where(x => !x.IsDeleted)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages);
            if (nameSearch != null)
            {
                query = query.Where(x => x.TourTemplate.TourName.Contains(nameSearch));
            }
            if (codeSearch != null)
            {
                query = query.Where(x => x.TourTemplate.Code.Contains(codeSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => x.TourTemplate.TourTemplateProvinces.Any(y => provinceIds.Contains(y.ProvinceId)));
            }
            if (tourCategoryIds?.Count > 0)
            {
                query = query.Where(x => tourCategoryIds.Contains(x.TourTemplate.TourCategoryId));
            }
            if (durationIds?.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.TourTemplate.DurationId));
            }
            if (status != null)
            {
                query = query.Where(x => status == x.Status);
            }
            if (startDateFrom != null)
            {
                query = query.Where(x => startDateFrom <= x.StartDate);
            }
            if (startDateTo != null)
            {
                query = query.Where(x => startDateTo >= x.StartDate);
            }
            int count = await query.CountAsync();
            List<TourPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .Select(x => new TourPreviewDTO
                {
                    TourId = x.TourId,
                    TourTemplateId = x.TourTemplateId,
                    Code = x.TourTemplate.Code,
                    TourName = x.TourTemplate.TourName,
                    Duration = x.TourTemplate.TourDuration.DurationName,
                    ImageUrl = x.TourTemplate.TourTemplateImages.FirstOrDefault().ImageUrl,
                    StartLocation = x.StartLocation,
                    StartDate = x.StartDate,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    MinParticipant = x.MinParticipant,
                    CurrentParticipant = x.CurrentParticipant,
                    Status = x.Status
                })
                .ToListAsync();
            return (count, items);
        }

        public async Task<TourDetailDTO?> GetTourById(string id)
        {
            return await _unitOfWork.TourRepository
                .Query()
                .Include(x => x.TourTemplate)
                .Include(x => x.TourPrices)
                .Include(x => x.TourRefundPolicies)
                .Include(x => x.TourBookings)
                .Where(x => !x.IsDeleted)
                .Select(x => new TourDetailDTO
                {
                    TourId = x.TourId,
                    TourTemplateId = x.TourTemplateId,
                    StartLocation = x.StartLocation,
                    StartDate = x.StartDate,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    MinParticipant = x.MinParticipant,
                    CurrentParticipant = x.CurrentParticipant,
                    Status = x.Status,
                    DepositPercent = x.DepositPercent,
                    RegisterOpenDate = x.RegisterOpenDate,
                    RegisterCloseDate = x.RegisterCloseDate,
                    CreatedAt = x.CreatedAt,
                    TotalBookings = x.TourBookings.Count,
                    TourPrices = x.TourPrices.Select(y => new TourPriceDTO
                    {
                        AgeFrom = y.AgeFrom,
                        AgeTo = y.AgeTo,
                        Name = y.Name,
                        Price = y.Price
                    }).ToList(),
                    TourPolicies = x.TourRefundPolicies.Select(y => new TourPolicyPreviewDTO
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent
                    }).ToList(),
                    PaymentDeadline = x.PaymentDeadline
                })
                .SingleOrDefaultAsync(x => x.TourId.Equals(id));
        }

        public async Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourRepository
                .Query();
            int count = await query.CountAsync();
            List<Tour> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .Where(x => x.Status == TourStatus.Opened)
                .ToListAsync();
            return (count, items);
        }

        public async Task<List<TourDetailDTO>> GetAllToursByTemplateIdsAsync(
            string tourTemplateId)
        {
            List<TourDetailDTO> items = await _unitOfWork.TourRepository.Query()
                .Include(x => x.TourTemplate)
                .Include(x => x.TourPrices)
                .Include(x => x.TourRefundPolicies)
                .Include(x => x.TourBookings)
                .Where(x => x.TourTemplateId.Equals(tourTemplateId) && x.IsDeleted == false)
                .Select(x => new TourDetailDTO
                {
                    TourId = x.TourId,
                    TourTemplateId = x.TourTemplateId,
                    StartLocation = x.StartLocation,
                    StartDate = x.StartDate,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    MinParticipant = x.MinParticipant,
                    CurrentParticipant = x.CurrentParticipant,
                    Status = x.Status,
                    RegisterOpenDate = x.RegisterOpenDate,
                    DepositPercent = x.DepositPercent,
                    RegisterCloseDate = x.RegisterCloseDate,
                    CreatedAt = x.CreatedAt,
                    TotalBookings = x.TourBookings.Count,
                    TourPrices = x.TourPrices.Select(y => new TourPriceDTO
                    {
                        AgeFrom = y.AgeFrom,
                        AgeTo = y.AgeTo,
                        Name = y.Name,
                        Price = y.Price
                    }).ToList(),
                    TourPolicies = x.TourRefundPolicies.Select(y => new TourPolicyPreviewDTO
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent
                    }).ToList()
                })
                .ToListAsync();
            return items;
        }

        public async Task ChangeTourStatusAsync(string tourId, string accountId, TourStatus tourStatus, string? reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && x.Role == UserRole.Manager) ??
                    throw new UnauthorizedException("UNAUTHORIZED");
                Tour? tour = await _unitOfWork.TourRepository.Query()
                    .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                    throw new ResourceNotFoundException("NOT_EXIST_TOUR");

                bool isManagerAcceptedOrDenyPendingTour = (TourStatus.Accepted == tourStatus || TourStatus.Rejected == tourStatus) &&
                    UserRole.Manager == account.Role && TourStatus.Pending == tour.Status;

                bool isRegisteredOpenedDatePass = ((DateTime)tour.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && TourStatus.Accepted == tourStatus;
                bool isRegisteredClosedDatePass = ((DateTime)tour.RegisterCloseDate).Date <= _timeZoneHelper.GetUTC7Now().Date && TourStatus.Pending == tour.Status;
                if (false == isManagerAcceptedOrDenyPendingTour)
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                if (isRegisteredOpenedDatePass && tourStatus == TourStatus.Accepted)
                {
                    tour.Status = TourStatus.Opened;
                }
                else if (false == isRegisteredClosedDatePass)
                {
                    tour.Status = tourStatus == TourStatus.Accepted ? TourStatus.Accepted : TourStatus.Rejected;
                }
                else
                {
                    throw new InvalidActionException("INVALID_ACTION_TOUR_STATUS_CHANGE");
                }
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                string historyId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = tourId,
                    Action = EntityModifyAction.ChangeStatus,
                    EntityType = EntityType.Tour,
                    Id = historyId,
                    ModifierRole = UserRole.Manager,
                    ModifiedBy = accountId,
                    Reason = reason,
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    StatusHistory = new EntityStatusHistory
                    {
                        Id = historyId,
                        OldStatus = (int)TourStatus.Pending,
                        NewStatus = (int)tour.Status,
                    }
                });
                await _unitOfWork.CommitTransactionAsync();
                if (tour.Status == TourStatus.Accepted)
                {
                    _backgroundJobClient.Schedule<ITourJob>(x => x.OpenTourAsync(tourId),_timeZoneHelper.GetLocalTimeFromUTC7(tour.RegisterOpenDate!.Value));
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CancelTourAsync(string tourId, string managerId, string? reason)
        {
            try
            {
                Manager? manager = await _unitOfWork.ManagerRepository.Query()
                    .SingleOrDefaultAsync(x => x.ManagerId.Equals(managerId)) ??
                    throw new UnauthorizedException("UNAUTHORIZED");

                Tour? tour = await _unitOfWork.TourRepository.Query().Include(x => x.TourBookings)
                    .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                    throw new ResourceNotFoundException("NOT_EXIST_TOUR");

                if (TourStatus.Accepted != tour.Status && TourStatus.Opened != tour.Status && TourStatus.Closed == tour.Status)
                {
                    throw new InvalidActionException("INVALID_ACTION_TOUR_CANCEL");
                }
                int oldStatus = (int)tour.Status;
                tour.Status = TourStatus.Cancelled;
                tour.CurrentParticipant = 0;

                await _unitOfWork.BeginTransactionAsync();
                string entityHistoryId = _idGenerator.GenerateId();
                await _unitOfWork.EntityStatusHistoryRepository.CreateAsync(new EntityStatusHistory()
                {
                    Id = entityHistoryId,
                    OldStatus = oldStatus,
                    NewStatus = (int)tour.Status,
                    EntityHistory = new EntityHistory()
                    {
                        Id = entityHistoryId,
                        Action = EntityModifyAction.ChangeStatus,
                        EntityId = tourId,
                        EntityType = EntityType.Tour,
                        Timestamp = _timeZoneHelper.GetUTC7Now(),
                        ModifiedBy = managerId,
                        ModifierRole = UserRole.Manager,
                        Reason = reason,
                    }
                });
                if (!tour.TourBookings.IsNullOrEmpty())
                {
                    foreach (var booking in tour.TourBookings)
                    {
                        int oldBookingStatus = (int)booking.Status;
                        booking.Status = BookingStatus.Cancelled;

                        string bookingHistoryId = _idGenerator.GenerateId();
                        await _unitOfWork.EntityStatusHistoryRepository.CreateAsync(new EntityStatusHistory()
                        {
                            Id = bookingHistoryId,
                            OldStatus = oldBookingStatus,
                            NewStatus = (int)booking.Status,
                            EntityHistory = new EntityHistory()
                            {
                                Id = bookingHistoryId,
                                Action = EntityModifyAction.ChangeStatus,
                                EntityId = booking.BookingId,
                                EntityType = EntityType.Booking,
                                Timestamp = _timeZoneHelper.GetUTC7Now(),
                                ModifiedBy = managerId,
                                ModifierRole = UserRole.Manager,
                                Reason = $"tour bị hủy vì {reason}",
                            }
                        });

                        if (booking.PaidAmount > 0)
                        {
                            await _unitOfWork.BookingRefundRepository.CreateAsync(new BookingRefund()
                            {
                                RefundId = _idGenerator.GenerateId(),
                                BookingId = booking.BookingId,
                                BankCode = null,
                                BankTransactionNumber = null,
                                CreatedAt = _timeZoneHelper.GetUTC7Now(),
                                RefundAmount = booking.PaidAmount,
                                RefundDate = null,
                                RefundNote = null,
                                RefundReason = reason,
                                RefundStatus = RefundStatus.Pending,
                            });
                        }
                    }
                }
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
                foreach (var booking in tour.TourBookings)
                {
                    _backgroundJobClient.Enqueue<IEmailJob>(x => x.SendSystemCancellationEmail(booking.BookingId, reason));
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteTourAsync(string tourId, string accountId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_TOUR");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourRepository.SoftDeleteAsync(tour);
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
