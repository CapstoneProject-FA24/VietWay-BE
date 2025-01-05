using System.Net.Http.Json;
using System.Text.Json;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookClient(string pageId, string pageToken, HttpClient httpClient)
    {
        private readonly string _pageId = pageId;
        private readonly string _pageToken = pageToken;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> PublishPostAsync(string content, string? url)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_pageId}/feed?access_token={_pageToken}", new
            {
                message = content,
                link = url,
                published = true
            });
            response.EnsureSuccessStatusCode();
            using JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("id").GetString();
        }
        public async Task<int> GetPostCommentCountAsync(string facebookPostId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{facebookPostId}?fields=comments.summary(total_count)&access_token={_pageToken}");
                response.EnsureSuccessStatusCode();
                using JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = jsonDocument.RootElement;
                return root.GetProperty("comments").GetProperty("summary").GetProperty("total_count").GetInt32();
            }
            catch
            {
                return 0;
            }
        }
        public async Task<int> GetPostShareCountAsync(string facebookPostId)
        {
            try 
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{facebookPostId}?fields=shares&access_token={_pageToken}");
                response.EnsureSuccessStatusCode();
                using JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = jsonDocument.RootElement;
                return root.GetProperty("shares").GetProperty("count").GetInt32();
            }
            catch
            {
                return 0;
            }
        }
        public async Task<int> GetPostImpressionCountAsync(string facebookPostId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{facebookPostId}?fields=insights.metric(post_impressions_unique)&access_token={_pageToken}");
                response.EnsureSuccessStatusCode();
                using JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = jsonDocument.RootElement;
                return root.GetProperty("insights").GetProperty("data")[0].GetProperty("values")[0].GetProperty("value").GetInt32();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<PostReaction> GetPostReactionCountByTypeAsync(string facebookPostId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{facebookPostId}/insights?metric=post_reactions_by_type_total&access_token={_pageToken}");
                response.EnsureSuccessStatusCode();
                using JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = jsonDocument.RootElement;
                string value = root.GetProperty("data")[0].GetProperty("values")[0].GetProperty("value").GetRawText();
                return JsonSerializer.Deserialize<PostReaction>(value);
            }
            catch
            {
                return new PostReaction();
            }
        }
    }
}