using System;
using System.IO;
using System.Net.Http;
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

        public async Task<string> PostTweetAsync(PostTweetRequestDTO postTweetRequestDTO)
        {
            var client = new TwitterClient(_xApiKey, _xApiKeySecret, _xAccessToken, _xAccessTokenSecret);

            // Step 1: Download the image
            byte[] imageData;
            using (var httpClient = new HttpClient())
            {
                imageData = await httpClient.GetByteArrayAsync(postTweetRequestDTO.ImageUrl);
            }

            // Step 2: Upload the image to Twitter
            var uploadedImage = await client.Upload.UploadBinaryAsync(imageData);
            if (uploadedImage == null)
                throw new Exception("Image upload failed");

            string mediaId = uploadedImage.Id.ToString();

            // Step 3: Post the tweet with the media_id
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
    }
}
