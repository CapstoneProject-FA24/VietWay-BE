using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiClient(string apiKey, string apiEndpoint, string? systemPrompt = null)
    {
        private readonly string _apiKey = apiKey;
        private readonly string _apiEndpoint = apiEndpoint;
        private readonly string? _systemPrompt = systemPrompt;

        public async Task<string> QueryAsync(string content)
        {
            using HttpClient httpClient = new();
            GeminiChatRequest chatRequest = new()
            {
                Contents =
                [
                    new()
                        {
                            Parts =
                            [
                                new() { Text = content }
                            ],
                            Role = "user"
                        }
                ],
            };
            if (_systemPrompt != null)
            {
                chatRequest.SystemInstruction = new SystemInstruction { Parts = new Part() { Text = _systemPrompt } };
            }
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_apiEndpoint}:generateContent?key={_apiKey}", chatRequest);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }

        public async Task<string> ChatAsync(List<Content> contents)
        {
            using HttpClient httpClient = new();
            GeminiChatRequest chatRequest = new()
            {
                Contents = contents
            };
            if (_systemPrompt != null)
            {
                chatRequest.SystemInstruction = new SystemInstruction { Parts = new Part() { Text = _systemPrompt } };
            }
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_apiEndpoint}:generateContent?key={_apiKey}",chatRequest);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }
    }
}
