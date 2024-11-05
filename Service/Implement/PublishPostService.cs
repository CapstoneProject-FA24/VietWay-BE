using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Management.Implement
{
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<string> PostTweetWithXAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            PostTweetRequestDTO postTweetRequestDTO = new PostTweetRequestDTO
            {
                Text = $"{post.Title}\n\n{post.Description}\n\nXem thêm tại: http://localhost:5174/bai-viet/{post.PostId}",
                ImageUrl = post.ImageUrl
            };
            return await _twitterService.PostTweetAsync(postTweetRequestDTO);
        }
    }
}
