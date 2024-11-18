using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.Twitter
{
    public class TwitterService : ITwitterService
    {
        private readonly string _xApiKey = Environment.GetEnvironmentVariable("X_API_KEY")
            ?? throw new Exception("X_API_KEY is not set in environment variables");
        private readonly string _xApiKeySecret = Environment.GetEnvironmentVariable("X_API_KEY_SECRET")
            ?? throw new Exception("X_API_KEY_SECRET is not set in environment variables");
        private readonly string _xAccessToken = Environment.GetEnvironmentVariable("X_ACCESS_TOKEN")
            ?? throw new Exception("X_ACCESS_TOKEN is not set in environment variables");
        private readonly string _xAccessTokenSecret = Environment.GetEnvironmentVariable("X_ACCESS_TOKEN_SECRET")
            ?? throw new Exception("X_ACCESS_TOKEN_SECRET is not set in environment variables");
        private readonly string _bearerToken = Environment.GetEnvironmentVariable("X_BEARER_TOKEN")
            ?? throw new Exception("X_BEARER_TOKEN is not set in environment variables");

        public async Task<string> PostTweetAsync(PostTweetRequestDTO postTweetRequestDTO)
        {
            var client = new TwitterClient(_xApiKey, _xApiKeySecret, _xAccessToken, _xAccessTokenSecret);

            byte[] imageData;
            using (var httpClient = new HttpClient())
            {
                imageData = await httpClient.GetByteArrayAsync(postTweetRequestDTO.ImageUrl);
            }

            var uploadedImage = await client.Upload.UploadBinaryAsync(imageData);
            if (uploadedImage == null)
                throw new Exception("Image upload failed");

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

        public async Task<string> GetTweetsAsync(string tweetIds)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
                var response = await httpClient.GetAsync($"https://api.twitter.com/2/tweets?ids={tweetIds}&tweet.fields=public_metrics");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch tweets: {response.ReasonPhrase}");
                }

                return await response.Content.ReadAsStringAsync();
            }
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
