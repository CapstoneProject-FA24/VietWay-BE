using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty.Redis;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiService(GeminiApiConfig config, HttpClient httpClient, IRedisCacheService redisCacheService) : IGeminiService
    {
        private readonly string? _systemPrompt = config.SystemPrompt;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly GeminiClient _geminiClient = new(config.ApiKey, httpClient);
        public async Task<string> ChatAsync(List<Content> contents)
        {
            _geminiClient.SystemPrompt = await GetSystemPrompt();
            return await _geminiClient.ChatAsync(contents);
        }

        public async Task<string> QueryAsync(string content)
        {
            _geminiClient.SystemPrompt = await GetSystemPrompt();
            return await _geminiClient.QueryAsync(content);
        }

        private async Task<string> GetSystemPrompt()
        {
            List<Province> provinces = await _redisCacheService.GetAsync<List<Province>>("Provinces") ?? [];
            List<TourCategory> tourCategories = await _redisCacheService.GetAsync<List<TourCategory>>("TourCategories") ?? [];
            List<TourDuration> tourDurations = await _redisCacheService.GetAsync<List<TourDuration>>("TourDurations") ?? [];

            string provinceNames = string.Join(", ", provinces.Select(x => x.Name).ToList());
            string tourCategoryNames = string.Join(", ", tourCategories.Select(x => x.Name).ToList());
            string tourDurationNames = string.Join(", ", tourDurations.Select(x => x.DurationName).ToList());
            string systemPrompt = _systemPrompt ?? ""
                .Replace("{{{provinceName}}}", provinceNames)
                .Replace("{{{tourCategory}}}", tourCategoryNames)
                .Replace("{{{tourDuration}}}", tourDurationNames);
            return systemPrompt;
        }
    }
}
