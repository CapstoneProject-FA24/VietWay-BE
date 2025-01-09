using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Management.Implement
{
    public class PopularService : IPopularService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private const string REDIS_KEY = "popularProvinces";
        private const string ATTRACTION_CATEGORY_REDIS_KEY = "popularAttractionCategories";
        private const string POST_CATEGORY_REDIS_KEY = "popularPostCategories";
        private const string TOUR_CATEGORY_REDIS_KEY = "popularTourCategories";
        private const string HASHTAG_REDIS_KEY = "popularHashtag";

        public PopularService(
            IUnitOfWork unitOfWork,
            IRedisCacheService redisCacheService,
            ITimeZoneHelper timeZoneHelper)
        {
            _unitOfWork = unitOfWork;
            _redisCacheService = redisCacheService;
            _timeZoneHelper = timeZoneHelper;
        }

        public async Task<List<string>> GetPopularProvincesAsync()
        {
            var rawCategories = await _redisCacheService.GetAsync<List<string>>(REDIS_KEY);
            var categories = new List<string>();

            if (rawCategories != null && rawCategories.Any())
            {
                return rawCategories.Take(10).ToList();
            }
            else
            {
                categories = await _unitOfWork.ProvinceRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Take(10)
                    .Select(x => x.ProvinceId!)
                    .ToListAsync();

                await _redisCacheService.SetAsync(REDIS_KEY, categories);
            }

            return categories;
        }

        public async Task CachePopularProvincesAsync()
        {
            try
            {
                var provinces = await _unitOfWork.ProvinceRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Select(x => new PopularProvinceDTO(x.ProvinceId ?? string.Empty))
                    .ToListAsync();

                if (provinces != null && provinces.Any())
                {
                    // Store only the IDs in Redis
                    var provinceIds = provinces.Select(p => p.ProvinceId).ToList();
                    await _redisCacheService.SetAsync(
                        REDIS_KEY,
                        provinceIds
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<string>> GetPopularAttractionCategoriesAsync()
        {
            var rawCategories = await _redisCacheService.GetAsync<List<string>>(ATTRACTION_CATEGORY_REDIS_KEY);
            var categories = new List<string>();

            if (rawCategories != null && rawCategories.Any())
            {
                return rawCategories.Take(10).ToList();
            }
            else
            {
                categories = await _unitOfWork.AttractionCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Take(10)
                    .Select(x => x.AttractionCategoryId!)
                    .ToListAsync();

                await _redisCacheService.SetAsync(ATTRACTION_CATEGORY_REDIS_KEY, categories);
            }

            return categories;
        }

        public async Task CachePopularAttractionCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.AttractionCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Select(x => new PopularAttractionCategoryDTO(x.AttractionCategoryId ?? string.Empty))
                    .ToListAsync();

                if (categories != null && categories.Any())
                {
                    // Store only the IDs in Redis
                    var categoryIds = categories.Select(p => p.AttractionCategoryId).ToList();
                    await _redisCacheService.SetAsync(
                        ATTRACTION_CATEGORY_REDIS_KEY,
                        categoryIds
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<string>> GetPopularPostCategoriesAsync()
        {
            var rawCategories = await _redisCacheService.GetAsync<List<string>>(POST_CATEGORY_REDIS_KEY);
            var categories = new List<string>();

            if (rawCategories != null && rawCategories.Any())
            {
                return rawCategories.Take(10).ToList();
            }
            else
            {
                categories = await _unitOfWork.PostCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Take(10)
                    .Select(x => x.PostCategoryId!)
                    .ToListAsync();

                await _redisCacheService.SetAsync(POST_CATEGORY_REDIS_KEY, categories);
            }

            return categories;
        }

        public async Task CachePopularPostCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.PostCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Select(x => new PopularPostCategoryDTO(x.PostCategoryId ?? string.Empty))
                    .ToListAsync();

                if (categories != null && categories.Any())
                {
                    // Store only the IDs in Redis
                    var categoryIds = categories.Select(p => p.PostCategoryId).ToList();
                    await _redisCacheService.SetAsync(
                        POST_CATEGORY_REDIS_KEY,
                        categoryIds
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<string>> GetPopularTourCategoriesAsync()
        {
            var rawCategories = await _redisCacheService.GetAsync<List<string>>(TOUR_CATEGORY_REDIS_KEY);
            var categories = new List<string>();

            if (rawCategories != null && rawCategories.Any())
            {
                return rawCategories.Take(10).ToList();
            }
            else
            {
                categories = await _unitOfWork.TourCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Take(10)
                    .Select(x => x.TourCategoryId!)
                    .ToListAsync();

                await _redisCacheService.SetAsync(TOUR_CATEGORY_REDIS_KEY, categories);
            }

            return categories;
        }

        public async Task CachePopularTourCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.TourCategoryRepository.Query()
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => x.Name)
                    .Select(x => new PopularTourCategoryDTO(x.TourCategoryId ?? string.Empty))
                    .ToListAsync();

                if (categories != null && categories.Any())
                {
                    // Store only the IDs in Redis
                    var categoryIds = categories.Select(p => p.TourCategoryId).ToList();
                    await _redisCacheService.SetAsync(
                        TOUR_CATEGORY_REDIS_KEY,
                        categoryIds
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<string>> GetPopularHashtagsAsync()
        {
            try
            {
                var xPopularHashtag = await _redisCacheService.GetAsync<List<string>>(HASHTAG_REDIS_KEY);
                if (xPopularHashtag != null && xPopularHashtag.Any())
                {
                    return xPopularHashtag;
                }
                else
                {
                    var topHashtags = await _unitOfWork.SocialMediaPostHashtagRepository.Query()
                        .Include(smph => smph.Hashtag)
                        .GroupBy(smph => smph.Hashtag.HashtagId)
                        .Select(group => new
                        {
                            HashtagId = group.Key,
                            Count = group.Count()
                        })
                        .OrderByDescending(x => x.Count)
                        .Take(5)
                        .ToListAsync();

                    return topHashtags.Select(x => x.HashtagId).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
