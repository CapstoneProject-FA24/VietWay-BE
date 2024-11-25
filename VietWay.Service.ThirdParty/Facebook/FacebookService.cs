using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookService(HttpClient httpClient, FacebookApiConfig config) : IFacebookService
    {
        private readonly FacebookClient _facebookClient = new(config.PageId, config.PageAccessToken, httpClient);

        public async Task<int> GetPostCommentCountAsync(string facebookPostId) => await _facebookClient.GetPostCommentCountAsync(facebookPostId);

        public async Task<int> GetPostImpressionCountAsync(string facebookPostId) => await _facebookClient.GetPostImpressionCountAsync(facebookPostId);

        public async Task<PostReaction> GetPostReactionCountByTypeAsync(string facebookPostId) => await _facebookClient.GetPostReactionCountByTypeAsync(facebookPostId);

        public async Task<int> GetPostShareCountAsync(string facebookPostId) => await _facebookClient.GetPostShareCountAsync(facebookPostId);

        public Task<string> PublishPostAsync(string content, string? url) => _facebookClient.PublishPostAsync(content, url);
    }
}
