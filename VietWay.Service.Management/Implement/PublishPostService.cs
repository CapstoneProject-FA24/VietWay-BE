using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models.V2;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Facebook;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Management.Implement
{
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService, IFacebookService facebookService,
        IRedisCacheService redisCacheService, ITimeZoneHelper timeZoneHelper) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFacebookService _facebookService = facebookService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;

        public async Task<FacebookMetricsDTO> GetFacebookPostMetricsAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            List<SocialMediaPost> socialMediaPosts = await _unitOfWork.SocialMediaPostRepository.Query()
               .Where(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Facebook).ToListAsync();
            if (socialMediaPosts.Count <= 0)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_PUBLISHED");
            }

            Task<int> countCommentTask = _facebookService.GetPostCommentCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<int> countShareTask = _facebookService.GetPostShareCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<int> countImpressionTask = _facebookService.GetPostImpressionCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<PostReaction> getReactionsTask = _facebookService.GetPostReactionCountByTypeAsync(socialMediaPosts[0].SocialPostId!);
            await Task.WhenAll(countCommentTask, countImpressionTask, countShareTask, getReactionsTask);
            return new FacebookMetricsDTO
            {
                CommentCount = countCommentTask.Result,
                ImpressionCount = countImpressionTask.Result,
                PostReactions = getReactionsTask.Result,
                ShareCount = countShareTask.Result
            };
        }

        public async Task<List<TweetDTO>> GetPublishedTweetByIdAsync(string entityId, SocialMediaPostEntity entityType)
        {
            var socialMediaPosts = await _unitOfWork.SocialMediaPostRepository.Query()
                .Where(x => x.EntityId == entityId && x.Site == SocialMediaSite.Twitter && x.EntityType == entityType)
                .ToListAsync();
            if (socialMediaPosts.IsNullOrEmpty())
            {
                throw new ResourceNotFoundException($"{entityType.ToString().ToUpper()}_NOT_PUBLISHED");
            }

            List<TweetDTO> tweetDto = await _redisCacheService.GetAsync<List<TweetDTO>>($"{entityId}-{(int)entityType}");
            
            if (tweetDto == null)
            {
                tweetDto = new List<TweetDTO>();
            }

            /*if (tweetDto.IsNullOrEmpty())
            {
                throw new ResourceNotFoundException("NOT_EXISTED_TWEET");
            }*/

            foreach (var post in socialMediaPosts)
            {
                var tweet = tweetDto.FirstOrDefault(x => x.XTweetId == post.SocialPostId);
                if (tweet != null)
                {
                    tweet.CreatedAt = post.CreatedAt;
                }
                else
                {
                    tweetDto.Add(new TweetDTO
                    {
                        XTweetId = post.SocialPostId,
                        RetweetCount = 0,
                        ReplyCount = 0,
                        LikeCount = 0,
                        QuoteCount = 0,
                        BookmarkCount = 0,
                        ImpressionCount = 0,
                        CreatedAt = post.CreatedAt
                    });
                }
            }

            return tweetDto;
        }

        public async Task PublishPostWithXAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            /*bool isPublished = await _unitOfWork.SocialMediaPostRepository.Query()
                .AnyAsync(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Twitter);
            if (isPublished)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_PUBLISHED");
            }*/

            PostTweetRequestDTO postTweetRequestDTO = new()
            {
                Text = $"{post.Title.ToUpper()}\n\nXem thêm tại: https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}",
                ImageUrl = post.ImageUrl
            };
            string result = await _twitterService.PostTweetAsync(postTweetRequestDTO);
            using JsonDocument document = JsonDocument.Parse(result);
            string tweetId = document.RootElement.GetProperty("data").GetProperty("id").GetString();

            try
            {
                if (tweetId.IsNullOrEmpty())
                {
                    throw new ServerErrorException("Post tweet error");
                }
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = tweetId,
                    Site = SocialMediaSite.Twitter,
                    EntityType = SocialMediaPostEntity.Post,
                    EntityId = post.PostId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishAttractionWithXAsync(string attractionId)
        {
            Attraction? attraction = await _unitOfWork.AttractionRepository.Query()
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION");

            if (attraction.Status != AttractionStatus.Approved || attraction.IsDeleted)
            {
                throw new InvalidActionException("INVALID_ACTION_ATTRACTION_CANNOT_POST");
            }

            try
            {
                PostTweetRequestDTO postTweetRequestDTO = new()
                {
                    Text = $"{attraction.Name.ToUpper()} - Trải nghiệm {attraction.Province.Name} cùng Vietway\n\n📍 {attraction.Address}\n✨ Hãy cùng VietWay khám phá {attraction.Name} tại https://vietway.projectpioneer.id.vn/diem-tham-quan/{attraction.AttractionId}?ref=x",
                    ImageUrl = attraction.AttractionImages.Select(x => x.ImageUrl).FirstOrDefault()
                };
                string result = await _twitterService.PostTweetAsync(postTweetRequestDTO);
                using JsonDocument document = JsonDocument.Parse(result);
                string tweetId = document.RootElement.GetProperty("data").GetProperty("id").GetString();

                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = tweetId,
                    Site = SocialMediaSite.Twitter,
                    EntityType = SocialMediaPostEntity.Attraction,
                    EntityId = attraction.AttractionId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishTourTemplateWithXAsync(string tourTemplateId)
        {
            TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .Include(x => x.Tours)
                .Include(x => x.Province)
                .Include(x => x.TourDuration)
                .Include(x => x.TourTemplateImages)
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_TOUR_TEMPLATE");

            if (tourTemplate.Status != TourTemplateStatus.Approved ||
                tourTemplate.IsDeleted ||
                !tourTemplate.Tours.Any(y => y.Status == TourStatus.Opened && ((DateTime)y.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)y.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !y.IsDeleted))
            {
                throw new InvalidActionException("INVALID_ACTION_TOUR_TEMPLATE_CANNOT_POST");
            }

            decimal minPrice = tourTemplate.Tours.Where(x => x.Status == TourStatus.Opened && ((DateTime)x.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)x.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !x.IsDeleted)
                                    .Select(y => (decimal)y.DefaultTouristPrice).Min();
            CultureInfo vietnamCulture = new CultureInfo("vi-VN");
            string formattedPrice = minPrice.ToString("C0", vietnamCulture);

            try
            {
                PostTweetRequestDTO postTweetRequestDTO = new()
                {
                    Text = $"{tourTemplate.TourName.ToUpper()}\n\n- Thời lượng: {tourTemplate.TourDuration.DurationName}\n- Khởi hành từ: {tourTemplate.Province.Name}\n- Giá từ: {formattedPrice}\n- Đăng ký tại: https://vietway.projectpioneer.id.vn/tour-du-lich/{tourTemplate.TourTemplateId}?ref=x",
                    ImageUrl = tourTemplate.TourTemplateImages.Select(x => x.ImageUrl).FirstOrDefault()
                };
                string result = await _twitterService.PostTweetAsync(postTweetRequestDTO);
                using JsonDocument document = JsonDocument.Parse(result);
                string tweetId = document.RootElement.GetProperty("data").GetProperty("id").GetString();

                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = tweetId,
                    Site = SocialMediaSite.Twitter,
                    EntityType = SocialMediaPostEntity.TourTemplate,
                    EntityId = tourTemplate.TourTemplateId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishPostToFacebookPageAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            /*bool isPublished = await _unitOfWork.SocialMediaPostRepository.Query()
                .AnyAsync(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Facebook);
            if (isPublished)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_PUBLISHED");
            }*/

            string facebookPostId = await _facebookService.PublishPostAsync(post.Description, $"https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = facebookPostId,
                    Site = SocialMediaSite.Facebook,
                    EntityType = SocialMediaPostEntity.Post,
                    EntityId = post.PostId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
