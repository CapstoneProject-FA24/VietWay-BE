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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public async Task<IActionResult> UploadPostTwitterAsync(string postId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishPostWithXAsync(postId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish post to X successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("tour-template/{templateId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadTemplateTwitterAsync(string templateId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishTourTemplateWithXAsync(templateId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish template to X successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("attraction/{attractionId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAttractionTwitterAsync(string attractionId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishAttractionWithXAsync(attractionId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish attraction to X successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("post/{postId}/facebook")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadPostFacebookAsync(string postId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishPostToFacebookPageAsync(postId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish post to Facebook successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("attraction/{attractionId}/facebook")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAttractionFacebookAsync(string attractionId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishAttractionToFacebookPageAsync(attractionId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish attraction to Facebook successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("tour-template/{templateId}/facebook")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadTemplateFacebookAsync(string templateId, [FromBody] List<string> hashtagName)
        {
            await _publishPostService.PublishTourTemplateToFacebookPageAsync(templateId, hashtagName);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Publish tour template to Facebook successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{entityId}/facebook/metrics")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<FacebookMetricsDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFacebookReactionsAsync(string entityId, [Required][FromQuery] SocialMediaPostEntity entityType)
        {

            return Ok(new DefaultResponseModel<List<FacebookMetricsDTO>>
            {
                Message = "Get facebook reaction count successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = await _publishPostService.GetFacebookPostMetricsAsync(entityId, entityType)
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

        [HttpGet("hashtag")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHashtags()
        {
            return Ok(new DefaultResponseModel<List<HashtagDTO>>
            {
                Message = "Get twitter post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = await _publishPostService.GetHashtags()
            });
        }
    }
}
