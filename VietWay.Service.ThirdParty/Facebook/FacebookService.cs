using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookService : IFacebookService
    {
        private readonly FacebookClient _facebookClient;
        public FacebookService()
        {
            string pageId = Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ID") ?? 
                throw new Exception("FACEBOOK_PAGE_ID is not set in environment variables");
            string pageToken = Environment.GetEnvironmentVariable("FACEBOOK_PAGE_ACCESS_TOKEN") ??
                throw new Exception("FACEBOOK_PAGE_ACCESS_TOKEN is not set in environment variables");
            _facebookClient = new FacebookClient(pageId, pageToken);
        }

        public async Task<int> GetPostCommentCountAsync(string facebookPostId) => await _facebookClient.GetPostCommentCountAsync(facebookPostId);

        public async Task<int> GetPostImpressionCountAsync(string facebookPostId) => await _facebookClient.GetPostImpressionCountAsync(facebookPostId);

        public async Task<PostReaction> GetPostReactionCountByTypeAsync(string facebookPostId) => await _facebookClient.GetPostReactionCountByTypeAsync(facebookPostId);

        public async Task<int> GetPostShareCountAsync(string facebookPostId) => await _facebookClient.GetPostShareCountAsync(facebookPostId);

        public Task<string> PublishPostAsync(string content, string? url) => _facebookClient.PublishPostAsync(content, url);
    }
}
