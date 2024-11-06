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
        public async Task<int> GetPublishedPostReactionAsync(string facebookPostId)
        {
            return await _facebookClient.GetPublishedPostReactionAsync(facebookPostId);
        }

        public Task<string> PublishPostAsync(string content, string? url)
        {
            return _facebookClient.PublishPostAsync(content, url);
        }
    }
}
