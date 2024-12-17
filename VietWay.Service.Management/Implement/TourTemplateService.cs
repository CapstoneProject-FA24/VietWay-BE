using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Cloudinary;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class TourTemplateService(IUnitOfWork unitOfWork, IIdGenerator idGenerator,
        ICloudinaryService cloudinaryService, ITimeZoneHelper timeZoneHelper) : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<string> CreateTemplateAsync(TourTemplate tourTemplate)
        {
            tourTemplate.TourTemplateId = _idGenerator.GenerateId();
            tourTemplate.CreatedAt = _timeZoneHelper.GetUTC7Now();

            bool isDrafted = tourTemplate.Status.Equals(TourTemplateStatus.Draft);

            if (isDrafted == true)
            {
                foreach (var province in tourTemplate.TourTemplateProvinces ?? [])
                {
                    province.TourTemplateId = tourTemplate.TourTemplateId;
                }
                foreach (var schedule in tourTemplate.TourTemplateSchedules ?? [])
                {
                    schedule.TourTemplateId = tourTemplate.TourTemplateId;
                    foreach (var attractionSchedule in schedule.AttractionSchedules ?? [])
                    {
                        attractionSchedule.TourTemplateId = tourTemplate.TourTemplateId;
                    }
                }
            }
            else
            {
                var existingCode = await GetByCodeAsync(tourTemplate.Code);
                if (existingCode != null)
                {
                    throw new InvalidInfoException($"EXISTED_TOUR_TEMPLATE_CODE");
                }
                if (tourTemplate.MinPrice == 0 || tourTemplate.MaxPrice == 0)
                {
                    throw new InvalidInfoException("INVALID_INFO_PRICE");
                }
                else if (tourTemplate.MinPrice > tourTemplate.MaxPrice)
                {
                    throw new Exception("INVALID_INFO_PRICE");
                }
                foreach (var province in tourTemplate.TourTemplateProvinces ?? [])
                {
                    province.TourTemplateId = tourTemplate.TourTemplateId;
                }
                foreach (var schedule in tourTemplate.TourTemplateSchedules ?? [])
                {
                    schedule.TourTemplateId = tourTemplate.TourTemplateId;
                    foreach (var attractionSchedule in schedule.AttractionSchedules ?? [])
                    {
                        attractionSchedule.TourTemplateId = tourTemplate.TourTemplateId;
                    }
                }
            }

            await _unitOfWork.TourTemplateRepository.CreateAsync(tourTemplate);
            return tourTemplate.TourTemplateId;
        }
        private async Task<TourTemplate> GetByCodeAsync(string code)
        {
            return await _unitOfWork.TourTemplateRepository.Query()
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task DeleteTemplateAsync(string accountId, string tourTemplateId)
        {
            TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_TOUR_TEMPLATE");
            Account? account = await _unitOfWork.AccountRepository.Query()
                .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                throw new UnauthorizedException("UNAUTHORIZED");

            bool hasRelatedTour = await _unitOfWork.TourRepository.Query().AnyAsync(x => x.TourTemplateId.Equals(tourTemplateId));
            bool isStaff = account.Role.Equals(UserRole.Staff);
            bool isManager = account.Role.Equals(UserRole.Manager);
            bool isDraft = tourTemplate.Status.Equals(TourTemplateStatus.Draft);
            bool isPending = tourTemplate.Status.Equals(TourTemplateStatus.Pending);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (isStaff && isDraft)
                {
                    await _unitOfWork.TourTemplateRepository.DeleteAsync(tourTemplate);
                }
                else if (isStaff && isPending)
                {
                    throw new InvalidActionException("INVALID_ACTION_DELETE_PENDING_TOUR_TEMPLATE");
                }
                else if (isManager && !hasRelatedTour && !(isPending || isDraft))
                {
                    await _unitOfWork.TourTemplateRepository.SoftDeleteAsync(tourTemplate);
                }
                else if (isManager && tourTemplate.Status.Equals(TourTemplateStatus.Pending))
                {
                    throw new InvalidActionException("INVALID_ACTION_DELETE_PENDING_TOUR_TEMPLATE");
                }
                else if (isManager && hasRelatedTour)
                {
                    throw new InvalidDataException("INVALID_ACTION_DELETE_TOUR_TEMPLATE_HAS_TOUR");
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PaginatedList<TourTemplatePreviewDTO>> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            List<TourTemplateStatus>? statuses,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds != null && templateCategoryIds.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Select(x => x.ProvinceId).Any(p => provinceIds.Contains(p)));
            }
            if (durationIds != null && durationIds.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.DurationId));
            }
            if (statuses?.Count > 0)
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }
            int count = await query.CountAsync();
            List<TourTemplatePreviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourTemplatePreviewDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    TourName = x.TourName,
                    StartingProvince = x.StartingProvince,
                    Duration = x.TourDuration.DurationName,
                    TourCategory = x.TourCategory.Name,
                    Status = x.Status,
                    MinPrice = x.MinPrice,
                    MaxPrice = x.MaxPrice,
                    CreatedAt = x.CreatedAt,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().ImageUrl,
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    Transportation = x.Transportation
                })
                .ToListAsync();
            return new PaginatedList<TourTemplatePreviewDTO>
            {
                Total = count,
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<TourTemplateDetailDTO?> GetTemplateByIdAsync(string id)
        {
            return await _unitOfWork
                .TourTemplateRepository
                .Query()
                .Select(x => new TourTemplateDetailDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    TourName = x.TourName,
                    Description = x.Description,
                    Duration = new TourDurationDTO
                    {
                        DurationId = x.TourDuration.DurationId,
                        DurationName = x.TourDuration.DurationName
                    },
                    TourCategory = new TourCategoryDTO
                    {
                        TourCategoryId = x.TourCategory.TourCategoryId,
                        Name = x.TourCategory.Name
                    },
                    Note = x.Note,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    StartingProvince = new ProvinceBriefPreviewDTO
                    {
                        ProvinceId = x.Province.ProvinceId,
                        ProvinceName = x.Province.Name
                    },
                    Transportation = x.Transportation,
                    Provinces = x.TourTemplateProvinces.Select(y => new ProvinceBriefPreviewDTO
                    {
                        ProvinceId = y.Province.ProvinceId,
                        ProvinceName = y.Province.Name
                    }).ToList(),
                    Schedules = x.TourTemplateSchedules.Select(y => new ScheduleDTO
                    {
                        DayNumber = y.DayNumber,
                        Title = y.Title,
                        Description = y.Description,
                        Attractions = y.AttractionSchedules.Select(z => new AttractionPreviewDTO
                        {
                            AttractionId = z.Attraction.AttractionId,
                            Name = z.Attraction.Name
                        }).ToList()
                    }).ToList(),
                    Images = x.TourTemplateImages.Select(y => new ImageDTO
                    {
                        ImageId = y.ImageId,
                        ImageUrl = y.ImageUrl
                    }).ToList(),
                    MinPrice = x.MinPrice,
                    MaxPrice = x.MaxPrice
                })
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(id));
        }

        public async Task UpdateTemplateAsync(string tourTemplateId, TourTemplate newTourTemplate)
        {
            TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .Include(x => x.TourTemplateSchedules)
                .ThenInclude(x => x.AttractionSchedules)
                .ThenInclude(x => x.Attraction)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourDuration)
                .Include(x => x.TourCategory)
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_TOUR_TEMPLATE");

            if (tourTemplate.Status.Equals(TourTemplateStatus.Approved))
            {
                throw new InvalidActionException("INVALID_ACTION_TOUR_TEMPLATE_ALREADY_APPROVED");
            }

            tourTemplate.Code = newTourTemplate.Code;
            tourTemplate.TourName = newTourTemplate.TourName;
            tourTemplate.Description = newTourTemplate.Description;
            tourTemplate.DurationId = newTourTemplate.DurationId;
            tourTemplate.TourCategoryId = newTourTemplate.TourCategoryId;
            tourTemplate.Note = newTourTemplate.Note;
            tourTemplate.MinPrice = newTourTemplate.MinPrice;
            tourTemplate.MaxPrice = newTourTemplate.MaxPrice;
            tourTemplate.StartingProvince = newTourTemplate.StartingProvince;
            tourTemplate.Transportation = newTourTemplate.Transportation;
            tourTemplate.Status = newTourTemplate.Status;

            if (newTourTemplate.TourTemplateProvinces != null)
            {
                tourTemplate.TourTemplateProvinces?.Clear();

                foreach (TourTemplateProvince province in newTourTemplate.TourTemplateProvinces)
                {
                    tourTemplate.TourTemplateProvinces?
                        .Add(new TourTemplateProvince()
                        {
                            ProvinceId = province.ProvinceId,
                            TourTemplateId = tourTemplateId
                        });
                }
            }

            if (newTourTemplate.TourTemplateSchedules != null)
            {
                tourTemplate.TourTemplateSchedules?.Clear();

                foreach (TourTemplateSchedule schedule in newTourTemplate.TourTemplateSchedules)
                {
                    tourTemplate.TourTemplateSchedules?
                        .Add(new TourTemplateSchedule
                        {
                            TourTemplateId = tourTemplateId,
                            DayNumber = schedule.DayNumber,
                            Description = schedule.Description,
                            Title = schedule.Title,
                            AttractionSchedules = schedule.AttractionSchedules.Select(x => new AttractionSchedule
                            {
                                AttractionId = x.AttractionId,
                                DayNumber = schedule.DayNumber,
                                TourTemplateId = tourTemplateId
                            }).ToList()
                        });
                }
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateTemplateImageAsync(TourTemplate tourTemplate, List<IFormFile>? imageFiles, List<string>? removedImageIds)
        {
            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    using Stream stream = imageFile.OpenReadStream();
                    string imageId = _idGenerator.GenerateId();
                    TourTemplateImage attractionImage = new()
                    {
                        TourTemplateId = tourTemplate.TourTemplateId,
                        ImageId = imageId,
                        ImageUrl = _cloudinaryService.GetImage(imageId)
                    };
                    tourTemplate.TourTemplateImages.Add(attractionImage);
                }
            }
            if (removedImageIds?.Count > 0)
            {
                var removedImages = tourTemplate.TourTemplateImages
                    .Where(x => removedImageIds.Contains(x.ImageId))
                    .ToList();
                foreach (var image in removedImages)
                {
                    tourTemplate.TourTemplateImages.Remove(image);
                }
                //_ = await _cloudinaryService.DeleteImages(removedImages.Select(x => x.Image.PublicId));
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
            }
        }

        public async Task<PaginatedList<TourTemplate>> GetAllApprovedTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds != null && templateCategoryIds.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Select(x => x.ProvinceId).Any(p => provinceIds.Contains(p)));
            }
            if (durationIds != null && durationIds.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.DurationId));
            }
            int count = await query.CountAsync();
            List<TourTemplate> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Status == TourTemplateStatus.Approved)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .ToListAsync();
            return new PaginatedList<TourTemplate>
            {
                Total = count,
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<PaginatedList<TourTemplateWithTourInfoDTO>> GetAllTemplateWithActiveToursAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<int>? numberOfDay,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            decimal? minPrice,
            decimal? maxPrice,
            int pageSize,
            int pageIndex,
            string? tourId)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false &&
                            x.Tours.Any(y => y.StartDate >= _timeZoneHelper.GetUTC7Now() &&
                                             y.Status == TourStatus.Opened && ((DateTime)y.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)y.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !y.IsDeleted));
            if (false == string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds?.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Any(y => provinceIds.Contains(y.ProvinceId)));
            }
            if (numberOfDay?.Count > 0)
            {
                query = query.Where(x => numberOfDay.Contains(x.TourDuration.NumberOfDay));
            }
            if (startDateFrom != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate >= startDateFrom));
            }
            if (startDateTo != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate <= startDateTo));
            }
            if (minPrice != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice >= minPrice));
            }
            if (maxPrice != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice <= maxPrice));
            }
            if (tourId != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.TourId != tourId && x.StartDate >= _timeZoneHelper.GetUTC7Now() &&
                                             x.Status == TourStatus.Opened && ((DateTime)x.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date 
                                             && ((DateTime)x.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !x.IsDeleted));
            }

            int count = await query.CountAsync();

            List<TourTemplateWithTourInfoDTO> items = await query
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                    .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .Include(x => x.Tours)
                .ThenInclude(x => x.TourPrices)
                .Include(x => x.Tours)
                .ThenInclude(x => x.TourRefundPolicies)
                .OrderBy(x => x.TourCategoryId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourTemplateWithTourInfoDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    StartingProvince = x.Province.Name,
                    Duration = x.TourDuration.DurationName,
                    TourCategory = x.TourCategory.Name,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().ImageUrl,
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    TourName = x.TourName,
                    Description = x.Description,
                    Note = x.Note,
                    Transportation = x.Transportation,
                    Schedules = x.TourTemplateSchedules.Select(y => new ScheduleDTO
                    {
                        DayNumber = y.DayNumber,
                        Title = y.Title,
                        Description = y.Description,
                        Attractions = y.AttractionSchedules.Select(z => new AttractionPreviewDTO
                        {
                            AttractionId = z.AttractionId,
                            Name = z.Attraction.Name,
                        }).ToList()
                    }).ToList(),
                    Tours = x.Tours
                    .Where(a => a.Status == TourStatus.Opened && ((DateTime)a.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)a.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !a.IsDeleted)
                    .Where(a => string.IsNullOrEmpty(tourId) || a.TourId != tourId)
                    .Select(y => new TourInfoDTO
                    {
                        TourId = y.TourId,
                        StartLocation = y.StartLocation,
                        StartDate = y.StartDate,
                        DefaultTouristPrice = y.DefaultTouristPrice,
                        MaxParticipant = y.MaxParticipant,
                        MinParticipant = y.MinParticipant,
                        CurrentParticipant = y.CurrentParticipant,
                        DepositPercent = y.DepositPercent,
                        PaymentDeadline = y.PaymentDeadline,
                        TourPrices = y.TourPrices.Select(z => new TourPriceDTO
                        {
                            PriceId = z.PriceId,
                            Price = z.Price,
                            Name = z.Name,
                            AgeFrom = z.AgeFrom,
                            AgeTo = z.AgeTo
                        }).ToList(),
                        TourPolicies = y.TourRefundPolicies.Select(z => new TourPolicyPreviewDTO
                        {
                            CancelBefore = z.CancelBefore,
                            RefundPercent = z.RefundPercent
                        }).ToList(),
                    }).ToList(),
                })
                .ToListAsync();
            return new PaginatedList<TourTemplateWithTourInfoDTO>
            {
                Total = count,
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
        public async Task UpdateTourTemplateImageAsync(string tourTemplateId, string staffId, List<IFormFile>? newImages, List<string>? imageIdsToRemove)
        {
            TourTemplate tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .Include(x => x.TourTemplateImages).SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId))
                ?? throw new ResourceNotFoundException("NOT_EXIST_TOUR_TEMPLATE");
            try
            {
                var enqueuedJobs = new List<Action>();
                await _unitOfWork.BeginTransactionAsync();
                tourTemplate.TourTemplateImages ??= [];
                if (newImages != null)
                {
                    foreach (var imageFile in newImages)
                    {
                        string imageId = _idGenerator.GenerateId();
                        using MemoryStream memoryStream = new();
                        using Stream stream = imageFile.OpenReadStream();
                        await stream.CopyToAsync(memoryStream);
                        enqueuedJobs.Add(() => _cloudinaryService.UploadImageAsync(imageId, imageFile.FileName, memoryStream.ToArray()));
                        tourTemplate.TourTemplateImages.Add(new TourTemplateImage
                        {
                            TourTemplateId = tourTemplate.TourTemplateId,
                            ImageId = imageId,
                            ImageUrl = _cloudinaryService.GetImage(imageId)
                        });
                    }
                }
                List<TourTemplateImage>? imagesToRemove = null;
                if (imageIdsToRemove?.Count > 0)
                {
                    imagesToRemove = tourTemplate.TourTemplateImages
                        .Where(x => imageIdsToRemove.Contains(x.ImageId))
                        .ToList();
                    foreach (TourTemplateImage image in imagesToRemove)
                    {
                        tourTemplate.TourTemplateImages.Remove(image);
                    }
                    enqueuedJobs.Add(() => _cloudinaryService.DeleteImagesAsync(imageIdsToRemove));
                }
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);

                await _unitOfWork.CommitTransactionAsync();
                enqueuedJobs.ForEach(job => job.Invoke());
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ChangeTourTemplateStatusAsync(string tourTemplateId, string accountId, TourTemplateStatus templateStatus, string? reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                    throw new UnauthorizedException("UNAUTHORIZED");
                TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                    .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId)) ??
                    throw new ResourceNotFoundException("NOT_EXIST_TOUR_TEMPLATE");

                bool isManagerApproveOrDenyPendingTourTemplate = (TourTemplateStatus.Approved == templateStatus || TourTemplateStatus.Rejected == templateStatus) &&
                    UserRole.Manager == account.Role && TourTemplateStatus.Pending == tourTemplate.Status;
                bool isStaffSubmitDraftTourTemplateForPreview = (TourTemplateStatus.Pending == templateStatus) && UserRole.Staff == account.Role &&
                    TourTemplateStatus.Draft == tourTemplate.Status;

                if (isStaffSubmitDraftTourTemplateForPreview)
                {
                    tourTemplate.Status = TourTemplateStatus.Pending;
                }
                else if (isManagerApproveOrDenyPendingTourTemplate)
                {
                    tourTemplate.Status = templateStatus;
                }
                else
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
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
