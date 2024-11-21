namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiService(GeminiApiConfig config, HttpClient httpClient) : IGeminiService
    {
        private readonly GeminiClient _geminiClient = new(config.ApiKey, httpClient)
        {
            SystemPrompt = config.SystemPrompt
        };
        public async Task<string> ChatAsync(List<Content> contents)
        {
            return await _geminiClient.ChatAsync(contents);
        }

        public async Task<string> QueryAsync(string content)
        {
            return await _geminiClient.QueryAsync(content);
        }

    }
}
