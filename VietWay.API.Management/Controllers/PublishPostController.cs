using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using Microsoft.AspNetCore.Authorization;
using VietWay.API.Management.RequestModel;
using VietWay.Repository.EntityModel;
using VietWay.Util.TokenUtil;
using AutoMapper;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.ThirdParty.Twitter;
using Tweetinvi.Core.Web;
using VietWay.Service.Management.Implement;
using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.Controllers
{
    [Route("api/published-posts")]
    [ApiController]
    public class PublishPostController(
        ITokenHelper tokenHelper, 
        IMapper mapper,
        IPublishPostService publishPostService) : ControllerBase
    {
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IMapper _mapper = mapper;
        private readonly IPublishPostService _publishPostService = publishPostService;


        [HttpPost("post/{postId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadPostTwitterAsync(string postId)
        {
            await _publishPostService.PublishPostWithXAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post tweet successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("tour-template/{templateId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadTemplateTwitterAsync(string templateId)
        {
            await _publishPostService.PublishTourTemplateWithXAsync(templateId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post template successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("attraction/{attractionId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAttractionTwitterAsync(string attractionId)
        {
            await _publishPostService.PublishAttractionWithXAsync(attractionId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post attraction successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("post/{postId}/facebook")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadPostFacebookAsync(string postId)
        {
            await _publishPostService.PublishPostToFacebookPageAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post facebook successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{postId}/facebook/metrics")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<FacebookMetricsDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFacebookReactionsAsync(string postId)
        {

            return Ok(new DefaultResponseModel<FacebookMetricsDTO>
            {
                Message = "Get facebook reaction count successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = await _publishPostService.GetFacebookPostMetricsAsync(postId)
            });
        }

        [HttpGet("{entityId}/twitter/reactions")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTwitterPostByPostIdAsync(string entityId, [Required] [FromQuery]SocialMediaPostEntity entityType)
        {
            List<TweetDTO> result = await _publishPostService.GetPublishedTweetByIdAsync(entityId, entityType);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Get twitter post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
