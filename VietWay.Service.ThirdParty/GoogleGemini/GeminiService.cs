using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiService(GeminiApiConfig config, HttpClient httpClient, IRedisCacheService redisCacheService, IUnitOfWork unitOfWork) : IGeminiService
    {
        private readonly string? _systemPrompt = config.SystemPrompt;
        private readonly string? _extractionSystemPrompt = config.InfoExtractSystemPrompt;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly GeminiClient _geminiClient = new(config.ApiKey, httpClient);
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> ChatAsync(List<Content> contents)
        {
            _geminiClient.SystemPrompt = await GetExtractionSystemPrompt();

            string json = await _geminiClient.ChatAsync(contents);
            json = json.Replace("```json", "").Replace("```", "");
            TourExtractionResult result = FromJson(json);
            IQueryable<TourTemplate> query = _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.Status == TourTemplateStatus.Approved && x.IsDeleted == false && x.Tours.Any(x => x.Status == TourStatus.Opened));
            if (result.ProvinceIds.Length > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Any(p=>result.ProvinceIds.Contains(p.ProvinceId)));
            }
            if (result.TourCategoryIds.Length > 0)
            {
                query = query.Where(x => result.TourCategoryIds.Contains(x.TourCategoryId));
            }
            if (result.TourDurationIds.Length > 0)
            {
                query = query.Where(x => result.TourDurationIds.Contains(x.DurationId));
            }
            if (result.NumberOfParticipants.HasValue)
            {
                query = query.Where(x => x.Tours.Any(x=>x.CurrentParticipant + result.NumberOfParticipants <= x.MaxParticipant && x.Status == TourStatus.Opened));
            }
            if (result.BudgetMin.HasValue)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice >= result.BudgetMin && x.Status == TourStatus.Opened));
            }
            if (result.BudgetMax.HasValue)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice <= result.BudgetMax && x.Status == TourStatus.Opened));
            }
            if (result.StartDate.HasValue)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate >= result.StartDate && x.Status == TourStatus.Opened));
            }
            if (result.EndDate.HasValue)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate <= result.EndDate && x.Status == TourStatus.Opened));
            }
            var relatedTours = await query
                .Select(x => new
                {
                    x.TourTemplateId,
                    x.TourName,
                    CategoryName =x.TourCategory.Name,
                    TourDuration = x.TourDuration.DurationName,
                    PriceFrom = x.Tours.Where(x=>x.Status == TourStatus.Opened).Min(x => x.DefaultTouristPrice),
                    Attractions = x.TourTemplateSchedules.SelectMany(x=>x.AttractionSchedules).Select(x => x.Attraction.Name).ToList(),
                    Provinces = x.TourTemplateProvinces.Select(x => x.Province.Name).ToList(),
                    StartDates = x.Tours.Where(x => x.Status == TourStatus.Opened).Select(x => x.StartDate).ToList(),
                    TourUrl = $"https://vietway.projectpioneer.id.vn/tour-du-lich/{x.TourTemplateId}"
                })
                .Take(5)
                .ToListAsync();
            _geminiClient.SystemPrompt = await GetSystemPrompt(JsonSerializer.Serialize(relatedTours));
            return await _geminiClient.ChatAsync(contents);
        }

        public async Task<string> QueryAsync(string content)
        {
            _geminiClient.SystemPrompt = await GetSystemPrompt(null);
            return await _geminiClient.QueryAsync(content);
        }

        private async Task<string> GetExtractionSystemPrompt()
        {
            List<Province> provinces = await _redisCacheService.GetAsync<List<Province>>("Provinces") ?? [];
            List<TourCategory> tourCategories = await _redisCacheService.GetAsync<List<TourCategory>>("TourCategories") ?? [];
            List<TourDuration> tourDurations = await _redisCacheService.GetAsync<List<TourDuration>>("TourDurations") ?? [];

            string provinceNames = string.Join(", ", provinces.Select(x => $"\"{x.ProvinceId}\": {x.Name}").ToList());
            string tourCategoryNames = string.Join(", ", tourCategories.Select(x => $"\"{x.TourCategoryId}\": {x.Name}").ToList());
            string tourDurationNames = string.Join(", ", tourDurations.Select(x => $"\"{x.DurationId}\": {x.DurationName}({x.NumberOfDay} day(s))").ToList());
            string prompts = (_extractionSystemPrompt ?? "")
                .Replace("{{{provinces}}}", provinceNames)
                .Replace("{{{tourCategories}}}", tourCategoryNames)
                .Replace("{{{tourDurations}}}", tourDurationNames)
                .Replace("{{{provinceName1}}}", provinces.Select(x => x.Name).First())
                .Replace("{{{provinceName2}}}", provinces.Select(x => x.Name).Last())
                .Replace("{{{provinceId1}}}", provinces.Select(x => x.ProvinceId).First())
                .Replace("{{{provinceId2}}}", provinces.Select(x => x.ProvinceId).Last())
                .Replace("{{{tourCategoryId1}}}", tourCategories.Select(x => x.TourCategoryId).First())
                .Replace("{{{tourCategory1}}}", tourCategories.Select(x => x.Name).First());
            return prompts;
        }

        private async Task<string> GetSystemPrompt(string? relevantTours)
        {
            List<Province> provinces = await _redisCacheService.GetAsync<List<Province>>("Provinces") ?? [];
            List<TourCategory> tourCategories = await _redisCacheService.GetAsync<List<TourCategory>>("TourCategories") ?? [];
            List<TourDuration> tourDurations = await _redisCacheService.GetAsync<List<TourDuration>>("TourDurations") ?? [];

            string provinceNames = string.Join(", ", provinces.Select(x => x.Name).ToList());
            string tourCategoryNames = string.Join(", ", tourCategories.Select(x => x.Name).ToList());
            string tourDurationNames = string.Join(", ", tourDurations.Select(x => x.DurationName).ToList());
            string systemPrompt = (_systemPrompt ?? "")
                .Replace("{{{provinceName}}}", provinceNames)
                .Replace("{{{tourCategory}}}", tourCategoryNames)
                .Replace("{{{tourDuration}}}", tourDurationNames)
                .Replace("{{{relatedTours}}}", relevantTours ?? "");
            return systemPrompt;
        }
        private class TourExtractionResult {

            public string[] ProvinceIds { get; set; } = Array.Empty<string>();
            public string[] TourCategoryIds { get; set; } = Array.Empty<string>();
            public string[] TourDurationIds { get; set; } = Array.Empty<string>();
            public int? NumberOfParticipants { get; set; }
            public decimal? BudgetMin { get; set; }
            public decimal? BudgetMax { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        private static TourExtractionResult FromJson(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };
                var result = JsonSerializer.Deserialize<TourExtractionResult>(json, options);
                return result ?? new TourExtractionResult();
            }
            catch (JsonException)
            {
                // Return a new object with default values if deserialization fails
                return new TourExtractionResult();
            }
        }

    }
}
