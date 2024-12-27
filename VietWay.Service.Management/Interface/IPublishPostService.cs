using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.ThirdParty.Twitter;

namespace VietWay.Service.Management.Interface
{
    public interface IPublishPostService
    {
        public Task<FacebookMetricsDTO> GetFacebookPostMetricsAsync(string postId);
        public Task PublishPostWithXAsync(string postId);
        Task PublishPostToFacebookPageAsync(string postId);
        public Task<List<TweetDTO>> GetPublishedTweetByIdAsync(string entityId, SocialMediaPostEntity entityType);
        public Task PublishAttractionWithXAsync(string attractionId);
        public Task PublishTourTemplateWithXAsync(string tourTemplateId);
    }
}
