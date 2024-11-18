using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IPublishPostService
    {
        public Task<FacebookMetricsDTO> GetFacebookPostMetricsAsync(string postId);
        public Task PostTweetWithXAsync(string postId);
        Task PublishPostToFacebookPageAsync(string postId);
        public Task<List<TweetDTO>> GetPublishedTweetsAsync();
        public Task<TweetDTO> GetPublishedTweetByIdAsync(string postId);
        public Task DeleteTweetWithXAsync(string postId);
    }
}
