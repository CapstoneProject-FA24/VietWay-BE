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
        private const string ATTRACTION_REDIS_KEY = "popularAttractionCategories";
        private const string POST_CATEGORY_REDIS_KEY = "popularPostCategories";
        private const string TOUR_CATEGORY_REDIS_KEY = "popularTourCategories";
        private const string HASHTAG_REDIS_KEY = "hashtagCounts";

        public PopularService(
            IUnitOfWork unitOfWork,
            IRedisCacheService redisCacheService,
            ITimeZoneHelper timeZoneHelper)
        {
            _unitOfWork = unitOfWork;
            _redisCacheService = redisCacheService;
            _timeZoneHelper = timeZoneHelper;
        }

        public async Task<List<PopularProvinceDTO>> GetPopularProvincesAsync()
        {
            try
            {
                var rawProvinces = await _redisCacheService.GetAsync<List<string>>(REDIS_KEY);
                var provinces = new List<PopularProvinceDTO>();

                if (rawProvinces != null && rawProvinces.Any())
                {
                    provinces = rawProvinces.Select(id => new PopularProvinceDTO(id)).ToList();
                }
                else
                {
                    provinces = await _unitOfWork.ProvinceRepository.Query()
                        .Where(x => !x.IsDeleted)
                        .OrderBy(x => x.Name)
                        .Select(x => new PopularProvinceDTO(x.ProvinceId ?? string.Empty))
                        .ToListAsync();

                    if (provinces != null && provinces.Any())
                    {
                        var provinceIds = provinces.Select(p => p.ProvinceId).ToList();
                        await _redisCacheService.SetAsync(
                            REDIS_KEY,
                            provinceIds,
                            TimeSpan.FromDays(1)
                        );
                    }
                }

                return provinces.Take(10).ToList();
            }
            catch (Exception)
            {
                return new List<PopularProvinceDTO>();
            }
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
                        provinceIds,
                        TimeSpan.FromDays(1)
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PopularAttractionCategoryDTO>> GetPopularAttractionCategoriesAsync()
        {
            try
            {
                var rawCategories = await _redisCacheService.GetAsync<List<string>>(ATTRACTION_REDIS_KEY);
                var categories = new List<PopularAttractionCategoryDTO>();

                if (rawCategories != null && rawCategories.Any())
                {
                    categories = rawCategories.Select(id => new PopularAttractionCategoryDTO(id)).ToList();
                }
                else
                {
                    categories = await _unitOfWork.AttractionCategoryRepository.Query()
                        .Where(x => !x.IsDeleted)
                        .OrderBy(x => x.Name)
                        .Select(x => new PopularAttractionCategoryDTO(x.AttractionCategoryId ?? string.Empty))
                        .ToListAsync();

                    if (categories != null && categories.Any())
                    {
                        var categoryIds = categories.Select(p => p.AttractionCategoryId).ToList();
                        await _redisCacheService.SetAsync(
                            ATTRACTION_REDIS_KEY,
                            categoryIds,
                            TimeSpan.FromDays(1)
                        );
                    }
                }

                return categories.Take(10).ToList();
            }
            catch (Exception)
            {
                return new List<PopularAttractionCategoryDTO>();
            }
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
                        ATTRACTION_REDIS_KEY,
                        categoryIds,
                        TimeSpan.FromDays(1)
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PopularPostCategoryDTO>> GetPopularPostCategoriesAsync()
        {
            try
            {
                var rawCategories = await _redisCacheService.GetAsync<List<string>>(POST_CATEGORY_REDIS_KEY);
                var categories = new List<PopularPostCategoryDTO>();

                if (rawCategories != null && rawCategories.Any())
                {
                    categories = rawCategories.Select(id => new PopularPostCategoryDTO(id)).ToList();
                }
                else
                {
                    categories = await _unitOfWork.PostCategoryRepository.Query()
                        .Where(x => !x.IsDeleted)
                        .OrderBy(x => x.Name)
                        .Select(x => new PopularPostCategoryDTO(x.PostCategoryId ?? string.Empty))
                        .ToListAsync();

                    if (categories != null && categories.Any())
                    {
                        var categoryIds = categories.Select(p => p.PostCategoryId).ToList();
                        await _redisCacheService.SetAsync(
                            POST_CATEGORY_REDIS_KEY,
                            categoryIds,
                            TimeSpan.FromDays(1)
                        );
                    }
                }

                return categories.Take(10).ToList();
            }
            catch (Exception)
            {
                return new List<PopularPostCategoryDTO>();
            }
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
                        categoryIds,
                        TimeSpan.FromDays(1)
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
                return rawCategories;
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
                        categoryIds,
                        TimeSpan.FromDays(1)
                    );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<string>> GetPopularHashtagsAsync(bool isTwitter)
        {
            var hashtagCounts = await _redisCacheService.GetAsync<Dictionary<string, int>>(HASHTAG_REDIS_KEY);
            if (hashtagCounts != null && hashtagCounts.Any() && isTwitter)
            {
                var topHashtags = hashtagCounts
                    .OrderByDescending(kvp => kvp.Value)
                    .Take(5)
                    .Select(kvp => kvp.Key)
                    .ToList();

                return topHashtags;
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
    }
}
