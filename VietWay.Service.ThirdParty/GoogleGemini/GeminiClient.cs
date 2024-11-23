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
    public class GeminiClient(string apiKey, HttpClient httpClient)
    {
        private readonly string _apiKey = apiKey;
        private readonly HttpClient _httpClient = httpClient;

        public string? SystemPrompt { get; set; }

        public async Task<string> QueryAsync(string content)
        {
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
            if (SystemPrompt != null)
            {
                chatRequest.SystemInstruction = new SystemInstruction { Parts = new Part() { Text = SystemPrompt } };
            }
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($":generateContent?key={_apiKey}", chatRequest);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }

        public async Task<string> ChatAsync(List<Content> contents)
        {
            
            GeminiChatRequest chatRequest = new()
            {
                Contents = contents
            };
            if (SystemPrompt != null)
            {
                chatRequest.SystemInstruction = new SystemInstruction { Parts = new Part() { Text = SystemPrompt } };
            }
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($":generateContent?key={_apiKey}",chatRequest);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }
    }
}
