using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiClient(string apiKey, string apiEndpoint)
    {
        private readonly string _apiKey = apiKey;
        private readonly string _apiEndpoint = apiEndpoint;
        public async Task<string> QueryAsync(string content)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_apiEndpoint}:generateContent?key={_apiKey}", new
            {
                Contents = new List<Content>
                {
                    new()
                    {
                        Parts =
                        [
                            new() { Text = content }
                        ]
                    }
                },
            }, options: new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

        }
        public async Task<string> ChatAsync(List<Content> contents)
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_apiEndpoint}:generateContent?key={_apiKey}", new
            {
                Contents = contents
            });
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
        }
    }
}
