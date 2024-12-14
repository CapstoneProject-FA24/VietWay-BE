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
        public async Task<string> CreateTour(Tour tour)
        {
            try
            {
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
                await _unitOfWork.CommitTransactionAsync();
                return tour.TourId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task EditTour(string tourId, Tour updatedTour)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .Include(x => x.TourPrices)
                .Include(x => x.TourRefundPolicies)
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                throw new ResourceNotFoundException("Tour not found");
            switch (tour.Status)
            {
                case TourStatus.Opened when tour.CurrentParticipant != 0:
                    throw new InvalidInfoException("Cannot edit tour that already has participants");
                    break;
                case TourStatus.Closed:
                    throw new InvalidInfoException("Tour already closed");
                    break;
                case TourStatus.Completed:
                    throw new InvalidInfoException("Tour already completed");
                    break;
                case TourStatus.Cancelled:
                    throw new InvalidInfoException("Tour already cancelled");
                    break;
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
            tour.CreatedAt = DateTime.Now;
            tour.Status = TourStatus.Pending;

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
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                    throw new ResourceNotFoundException("Account not found");
                Tour? tour = await _unitOfWork.TourRepository.Query()
                    .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                    throw new ResourceNotFoundException("Tour not found");

                bool isManagerAcceptedOrDenyPendingTour = (TourStatus.Accepted == tourStatus || TourStatus.Rejected == tourStatus) &&
                    UserRole.Manager == account.Role && TourStatus.Pending == tour.Status;

                bool isRegisteredOpenedDatePass = ((DateTime)tour.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && TourStatus.Accepted == tourStatus;

                if (isManagerAcceptedOrDenyPendingTour)
                {
                    tour.Status = tourStatus;
                }
                else
                {
                    throw new UnauthorizedException("You are not allowed to perform this action");
                }

                if (isRegisteredOpenedDatePass)
                {
                    tour.Status = TourStatus.Opened;
                }

                await _unitOfWork.TourRepository.UpdateAsync(tour);
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
                    throw new UnauthorizedException("You are not allowed to perform this action");

                Tour? tour = await _unitOfWork.TourRepository.Query().Include(x => x.TourBookings)
                    .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                    throw new ResourceNotFoundException("Tour not found");

                if (TourStatus.Accepted != tour.Status && TourStatus.Opened != tour.Status && TourStatus.Closed == tour.Status)
                {
                    throw new InvalidOperationException("Cannot cancel tour that is not accepted, opened or closed");
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
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteTourAsync(string tourId)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                throw new ResourceNotFoundException("Tour not found");

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
