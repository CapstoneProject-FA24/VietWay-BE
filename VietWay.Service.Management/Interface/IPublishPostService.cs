﻿using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.ThirdParty.Twitter;

namespace VietWay.Service.Management.Interface
{
    public interface IPublishPostService
    {
        public Task<FacebookMetricsDTO> GetFacebookPostMetricsAsync(string postId);
        public Task PostTweetWithXAsync(string postId);
        Task PublishPostToFacebookPageAsync(string postId);
        public Task<List<TweetDTO>> GetPublishedTweetByIdAsync(string postId);
    }
}
