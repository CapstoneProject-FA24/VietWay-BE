using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public interface IFacebookService
    {
        public Task<string> PublishPostAsync(string content, string? url);
        public Task<int> GetPostCommentCountAsync(string facebookPostId);
        public Task<int> GetPostShareCountAsync(string facebookPostId);
        public Task<int> GetPostImpressionCountAsync(string facebookPostId);
        public Task<PostReaction> GetPostReactionCountByTypeAsync(string facebookPostId);
    }
}
