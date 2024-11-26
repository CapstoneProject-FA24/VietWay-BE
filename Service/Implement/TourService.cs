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

            /*if (tour.Status.Equals(TourStatus.Opened) && tour.CurrentParticipant != 0)
            {
                throw new Exception("Cannot edit tour that already have participant");
            }
            else if (tour.Status.Equals(TourStatus.Closed))
            {
                throw new Exception("Tour already closed");
            }
            else if (tour.Status.Equals(TourStatus.Completed))
            {
                throw new Exception("Tour already completed");
            }
            else if (tour.Status.Equals(TourStatus.Cancelled))
            {
                throw new Exception("Tour already cancelled");
            }*/

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

                bool isRegisteredOpenedDatePass = tour.RegisterOpenDate <= DateTime.Today && UserRole.Manager == account.Role;

                if (isManagerAcceptedOrDenyPendingTour)
                {
                    tour.Status = tourStatus;
                }
                else if (isRegisteredOpenedDatePass)
                {
                    tour.Status = TourStatus.Opened;
                }
                else
                {
                    throw new UnauthorizedException("You are not allowed to perform this action");
                }

                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
                if (tour.Status == TourStatus.Accepted)
                {
                    _backgroundJobClient.Schedule<ITourJob>(x=>x.OpenTourAsync(tourId),_timeZoneHelper.GetLocalTimeFromUTC7(tour.RegisterOpenDate!.Value));
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
