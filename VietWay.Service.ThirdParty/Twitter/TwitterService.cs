using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using VietWay.Repository.EntityModel;
using static Google.Apis.Requests.BatchRequest;

namespace VietWay.Service.ThirdParty.Twitter
{
    public class TwitterService(TwitterServiceConfiguration config) : ITwitterService
    {
        private readonly string _xApiKey = config.XApiKey;
        private readonly string _xApiKeySecret = config.XApiKeySecret;
        private readonly string _xAccessToken = config.XAccessToken;
        private readonly string _xAccessTokenSecret = config.XAccessTokenSecret;
        private readonly string _bearerToken = config.BearerToken;

        public async Task<string> PostTweetAsync(PostTweetRequestDTO postTweetRequestDTO)
        {
            var client = new TwitterClient(_xApiKey, _xApiKeySecret, _xAccessToken, _xAccessTokenSecret);

            byte[] imageData;
            using (var httpClient = new HttpClient())
            {
                imageData = await httpClient.GetByteArrayAsync(postTweetRequestDTO.ImageUrl);
            }

            var uploadedImage = await client.Upload.UploadBinaryAsync(imageData) ?? throw new Exception("Image upload failed");
            string mediaId = uploadedImage.Id.ToString();

            var result = await client.Execute.AdvanceRequestAsync(BuildTweetRequestWithMedia(postTweetRequestDTO, mediaId, client));
            return result.Content;
        }

        private static Action<ITwitterRequest> BuildTweetRequestWithMedia(
            PostTweetRequestDTO postTweetRequestDTO, string mediaId, TwitterClient client)
        {
            return (ITwitterRequest request) =>
            {
                // Add media_id to the request body
                var requestBody = new
                {
                    text = postTweetRequestDTO.Text,
                    media = new { media_ids = new[] { mediaId } }
                };

                var jsonBody = client.Json.Serialize(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                request.Query.Url = "https://api.twitter.com/2/tweets";
                request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                request.Query.HttpContent = content;
            };
        }

        public async Task<List<TweetDTO>> GetTweetsAsync(List<string> tweetIds)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            var response = await httpClient.GetAsync($"https://api.twitter.com/2/tweets?ids={tweetIds}&tweet.fields=public_metrics");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch tweets: {response.ReasonPhrase}");
            }
            using JsonDocument document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement tweetData = document.RootElement.GetProperty("data");
            List<TweetDTO> tweetDTOs = [];
            foreach (var tweet in tweetData.EnumerateArray())
            {
                var tweetDTO = new TweetDTO
                {
                    XTweetId = tweet.GetProperty("id").GetString(),
                    RetweetCount = tweet.GetProperty("public_metrics").GetProperty("retweet_count").GetInt32(),
                    ReplyCount = tweet.GetProperty("public_metrics").GetProperty("reply_count").GetInt32(),
                    LikeCount = tweet.GetProperty("public_metrics").GetProperty("like_count").GetInt32(),
                    QuoteCount = tweet.GetProperty("public_metrics").GetProperty("quote_count").GetInt32(),
                    BookmarkCount = tweet.GetProperty("public_metrics").GetProperty("bookmark_count").GetInt32(),
                    ImpressionCount = tweet.GetProperty("public_metrics").GetProperty("impression_count").GetInt32()
                };
                tweetDTOs.Add(tweetDTO);
            }
            return tweetDTOs;
        }

        public async Task<string> GetTweetByIdAsync(string tweetId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
                var response = await httpClient.GetAsync($"https://api.twitter.com/2/tweets/{tweetId}?tweet.fields=public_metrics");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch tweets: {response.ReasonPhrase}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task DeleteTweetAsync(string tweetId)
        {
            var client = new TwitterClient(_xApiKey, _xApiKeySecret, _xAccessToken, _xAccessTokenSecret);

            var result = await client.Execute.AdvanceRequestAsync(request =>
            {
                request.Query.Url = $"https://api.twitter.com/2/tweets/{tweetId}";
                request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.DELETE;
            });

            if (!result.Response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete tweet {tweetId}: {result.Response.ReasonPhrase}");
            }
        }
    }
}
