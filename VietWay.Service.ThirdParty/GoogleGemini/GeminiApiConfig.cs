using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiApiConfig(string apiKey, string? systemPrompt, string? infoExtractSystemPrompt)
    {
        private readonly string _apiKey = apiKey;
        private readonly string? _systemPrompt = systemPrompt;
        private readonly string? _infoExtractSystemPrompt = infoExtractSystemPrompt;
        public string ApiKey { get => _apiKey; }
        public string? SystemPrompt { get => _systemPrompt; }
        public string? InfoExtractSystemPrompt { get => _infoExtractSystemPrompt; }
    }
}
