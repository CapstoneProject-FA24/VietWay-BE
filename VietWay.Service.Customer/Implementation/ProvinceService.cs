using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class ProvinceService(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper) : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;

        public async Task<ProvinceWithImageDTO?> GetProvinceImagesAsync(string provinceId, int imageCount)
        {
            Province? province = await _unitOfWork.ProvinceRepository.Query()
                .Where(x => x.ProvinceId == provinceId)
                .FirstOrDefaultAsync();
            if (province == null)
            {
                return null;
            }
            List<ImageDTO> images = await _unitOfWork.ProvinceRepository.Query()
            .Where(x => x.ProvinceId == provinceId)
            .Select(x => new
            {
                AttractionImageDTOs = x.Attractions
                    .Where(y => !y.IsDeleted && y.Status == AttractionStatus.Approved)
                    .SelectMany(attraction => attraction.AttractionImages
                        .Select(image => new ImageDTO { ImageId = image.ImageId, Url = image.ImageUrl })
                    ),
                PostImageDTOs = x.Posts
                    .Where(post => !post.IsDeleted && post.Status == PostStatus.Approved)
                    .Select(post => new ImageDTO { ImageId = post.PostId, Url = post.ImageUrl })
            })
            .SelectMany(x => x.AttractionImageDTOs.Concat(x.PostImageDTOs))
            .Take(imageCount-1)
            .ToListAsync();
            images.Insert(0, new ImageDTO() { ImageId = province.ProvinceId, Url = province.ImageUrl });
            return new ProvinceWithImageDTO
            {
                ProvinceId = province.ProvinceId,
                Name = province.Name,
                Images = images
            };
        }

        public async Task<List<ProvincePreviewDTO>> GetProvinces()
        {
            return await _unitOfWork.ProvinceRepository.Query()
                .Where(x => false == x.IsDeleted)
                .Select(x => new ProvincePreviewDTO
                {
                    ProvinceId = x.ProvinceId,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<(int count, List<ProvinceDetailDTO> items)> GetProvincesDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize)
        {
            IQueryable<Province> query = _unitOfWork.ProvinceRepository.Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            int count = await query.CountAsync();
            List<ProvinceDetailDTO> result = await query
                .OrderBy(x => x.ProvinceId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProvinceDetailDTO
                {
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Name,
                    ImageUrl = x.ImageUrl,
                    AttractionsCount = x.Attractions
                        .Where(y => false == y.IsDeleted && AttractionStatus.Approved == y.Status).Count(),
                    PostsCount = x.Posts
                        .Where(y => false == y.IsDeleted && PostStatus.Approved == y.Status).Count(),
                    EventsCount = x.Events
                        .Where(y => false == y.IsDeleted && EventStatus.Approved == y.Status).Count(),
                    ToursCount = x.TourTemplateProvinces
                        .Where(y => false == y.TourTemplate.IsDeleted && TourTemplateStatus.Approved == y.TourTemplate.Status)
                        .Where(y => y.TourTemplate.Tours.Any(z => _timeZoneHelper.GetUTC7Now() <= z.StartDate && TourStatus.Opened == z.Status))
                        .Count()
                })
                .ToListAsync();
            return (count, result);
        }
    }
}
