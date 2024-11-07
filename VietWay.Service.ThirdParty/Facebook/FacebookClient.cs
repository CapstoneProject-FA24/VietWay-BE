using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookClient(string pageId, string pageToken)
    {
        private readonly string _pageId = pageId;
        private readonly string _pageToken = pageToken;
        private const string BASEURL = "https://graph.facebook.com/v21.0";

        public async Task<string> PublishPostAsync(string content, string? url)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{BASEURL}/{_pageId}/feed?access_token={_pageToken}", new
            {
                message = content,
                link = url,
                published = true
            });
            response.EnsureSuccessStatusCode();
            PublishPostResponse? postResponse = await response.Content.ReadFromJsonAsync<PublishPostResponse>();
            return postResponse!.Id;
        }
        public async Task<int> GetPublishedPostReactionAsync(string facebookPostId)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync($"{BASEURL}/{facebookPostId}?fields=reactions.summary(total_count)&access_token={_pageToken}");
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            try
            {
                return root.GetProperty("reactions").GetProperty("summary").GetProperty("total_count").GetInt32();
            }
            catch
            {
                return 0;
            }
        }
        internal class PublishPostResponse
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = default!;
        }
        internal class ReactionResponse
        {
            [JsonPropertyName("reactions")]
            public Reactions ReactionsInfo { get; set; } = new();
            [JsonPropertyName("id")]
            public string PostId { get; set; } = default!;
            internal class Reactions
            {
                [JsonPropertyName("data")]
                public List<ReactionData> Data { get; set; } = new();
                [JsonPropertyName("summary")]
                public ReactionSummary Summary { get; set; } = new();
                [JsonPropertyName("paging")]
                public ReactionPaging Paging { get; set; } = new();

                internal class ReactionData
                {
                    [JsonPropertyName("id")]
                    public string Id { get; set; } = default!;
                    [JsonPropertyName("name")]
                    public string Name { get; set; } = default!;
                    [JsonPropertyName("type")]
                    public string Type { get; set; } = default!;
                }
                internal class ReactionSummary
                {
                    [JsonPropertyName("total_count")]
                    public int TotalCount { get; set; }
                }
                internal class ReactionPaging
                {
                    [JsonPropertyName("cursors")]
                    public Cursor Cursors { get; set; } = new();

                    internal class Cursor
                    {
                        public string? Before { get; set; }
                        public string? After { get; set; }
                    }
                }
            }
        }
    }
}