using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiService : IGeminiService
    {
        private readonly GeminiClient _geminiClient;
        public GeminiService()
        {
            string apiKey = Environment.GetEnvironmentVariable("GEMINI_AI_API_KEY") ??
                throw new Exception("GEMINI_AI_API_KEY is not set in environment variables");
            string apiEndpoint = Environment.GetEnvironmentVariable("GEMINI_AI_API_ENDPOINT") ??
                throw new Exception("GEMINI_AI_API_ENDPOINT is not set in environment variables");
            string? systemPrompt = Environment.GetEnvironmentVariable("GEMINI_AI_SYSTEM_PROMPT");

            _geminiClient = new GeminiClient(apiKey, apiEndpoint, systemPrompt);
        }

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
