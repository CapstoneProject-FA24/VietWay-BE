using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using PostStatus = VietWay.Repository.EntityModel.Base.PostStatus;
using EventStatus = VietWay.Repository.EntityModel.Base.EventStatus;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.DateTimeUtil;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Util.IdUtil;
using VietWay.Util.CustomExceptions;
using Microsoft.AspNetCore.Http;
using VietWay.Service.ThirdParty.Cloudinary;
namespace VietWay.Service.Management.Implement
{
    public class ProvinceService(IUnitOfWork unitOfWork,
        IIdGenerator idGenerator,
        ICloudinaryService cloudinaryService,
        ITimeZoneHelper timeZoneHelper) : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        public async Task<(int totalCount , List<ProvincePreviewDTO> items)> GetAllProvinces(
            string? nameSearch,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .ProvinceRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }

            int count = await query.CountAsync();
            List<ProvincePreviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ProvincePreviewDTO
                {
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (count, items);
        }

        public async Task<Province?> GetProvinceById(string id)
        {
            return await _unitOfWork
                .ProvinceRepository
                .Query()
                .SingleOrDefaultAsync(x => x.ProvinceId.Equals(id));
        }

        public async Task<(int count, List<ProvinceDetailDTO>)> GetAllProvinceDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize)
        {
            IQueryable<Province> query = _unitOfWork.ProvinceRepository.Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
#warning implement sort province by zone
            int count = await query.CountAsync();
            List<ProvinceDetailDTO> result = await query
                .OrderBy(x => x.ProvinceId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProvinceDetailDTO
                {
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    AttractionsCount = x.Attractions
                        .Where(y => false == y.IsDeleted && AttractionStatus.Approved == y.Status).Count(),
                    PostsCount = x.Posts
                        .Where(y => false == y.IsDeleted && PostStatus.Approved == y.Status).Count(),
                    ToursCount = x.TourTemplateProvinces
                        .Where(y => false == y.TourTemplate.IsDeleted && TourTemplateStatus.Approved == y.TourTemplate.Status)
                        .Where(y => y.TourTemplate.Tours.Any(z => _timeZoneHelper.GetUTC7Now() <= z.StartDate && TourStatus.Opened == z.Status))
                        .Count()
                })
                .ToListAsync();
            return (count, result);
        }

        public async Task<string> CreateProvinceAsync(Province province)
        {
            try
            {
                province.ProvinceId ??= _idGenerator.GenerateId();
                province.CreatedAt = DateTime.Now;
                province.ImageUrl = "";
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ProvinceRepository.CreateAsync(province);
                await _unitOfWork.CommitTransactionAsync();
                return province.ProvinceId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateProvinceAsync(Province newProvince)
        {
            Province? province = await _unitOfWork.ProvinceRepository.Query()
                .SingleOrDefaultAsync(x => x.ProvinceId.Equals(newProvince.ProvinceId)) ??
                throw new ResourceNotFoundException("Province not found");

            province.Name = newProvince.Name;
            province.ImageUrl = newProvince.ImageUrl;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ProvinceRepository.UpdateAsync(province);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateProvinceImageAsync(string provinceId, IFormFile newImages)
        {
            Province province = await _unitOfWork.ProvinceRepository.Query()
                .SingleOrDefaultAsync(x => x.ProvinceId.Equals(provinceId))
                ?? throw new ResourceNotFoundException("Province not found");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (newImages != null)
                {
                    string imageId = $"{provinceId}-image-{_idGenerator.GenerateId()}";
                    using MemoryStream memoryStream = new();
                    using Stream stream = newImages.OpenReadStream();
                    await stream.CopyToAsync(memoryStream);
                    await _cloudinaryService.UploadImageAsync(imageId, newImages.FileName, memoryStream.ToArray());
                    province.ImageUrl = _cloudinaryService.GetImage(imageId);
                }

                await _unitOfWork.ProvinceRepository.UpdateAsync(province);
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
