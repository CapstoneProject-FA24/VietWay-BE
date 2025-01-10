using IdGen;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models;
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
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService, IFacebookService facebookService,
        IRedisCacheService redisCacheService, ITimeZoneHelper timeZoneHelper, IIdGenerator idGenerator) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFacebookService _facebookService = facebookService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;

        public async Task<List<FacebookMetricsDTO>> GetFacebookPostMetricsAsync(string entityId, SocialMediaPostEntity entityType)
        {
            var query = _unitOfWork.SocialMediaPostRepository.Query().Include(x => x.SocialMediaPostHashtags).ThenInclude(x => x.Hashtag);
            List<SocialMediaPost> socialMediaPosts = entityType switch
            {
                SocialMediaPostEntity.Post => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.Post && x.PostId == entityId && x.Site == SocialMediaSite.Facebook)
                    .ToListAsync(),
                SocialMediaPostEntity.Attraction => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.Attraction && x.AttractionId == entityId && x.Site == SocialMediaSite.Facebook)
                    .ToListAsync(),
                SocialMediaPostEntity.TourTemplate => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.TourTemplate && x.TourTemplateId == entityId && x.Site == SocialMediaSite.Facebook)
                    .ToListAsync(),
                _ => throw new InvalidActionException("INVALID_ACTION_ENTITY_TYPE")
            };
            if (socialMediaPosts.Count <= 0)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_PUBLISHED");
            }

            List<FacebookMetricsDTO> facebookMetrics = [];

            foreach (var socialMediaPost in socialMediaPosts)
            {
                Task<int> countCommentTask = _facebookService.GetPostCommentCountAsync(socialMediaPost.SocialPostId!);
                Task<int> countShareTask = _facebookService.GetPostShareCountAsync(socialMediaPost.SocialPostId!);
                Task<int> countImpressionTask = _facebookService.GetPostImpressionCountAsync(socialMediaPost.SocialPostId!);
                Task<PostReaction> getReactionsTask = _facebookService.GetPostReactionCountByTypeAsync(socialMediaPost.SocialPostId!);
                await Task.WhenAll(countCommentTask, countImpressionTask, countShareTask, getReactionsTask);
                facebookMetrics.Add(new FacebookMetricsDTO
                {
                    CommentCount = countCommentTask.Result,
                    ImpressionCount = countImpressionTask.Result,
                    PostReactions = getReactionsTask.Result,
                    ShareCount = countShareTask.Result,
                    CreatedAt = socialMediaPost.CreatedAt,
                    FacebookPostId = socialMediaPost.SocialPostId,
                    Hashtags = socialMediaPost.SocialMediaPostHashtags.Select(x => x.Hashtag.HashtagName).ToList()
                });
            }

            return facebookMetrics;
        }

        public async Task<List<TweetDTO>> GetPublishedTweetByIdAsync(string entityId, SocialMediaPostEntity entityType)
        {
            var query = _unitOfWork.SocialMediaPostRepository.Query().Include(x => x.SocialMediaPostHashtags).ThenInclude(x => x.Hashtag);
            List<SocialMediaPost> socialMediaPosts = entityType switch
            {
                SocialMediaPostEntity.Post => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.Post && x.PostId == entityId && x.Site == SocialMediaSite.Twitter)
                    .ToListAsync(),
                SocialMediaPostEntity.Attraction => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.Attraction && x.AttractionId == entityId && x.Site == SocialMediaSite.Twitter)
                    .ToListAsync(),
                SocialMediaPostEntity.TourTemplate => await query
                    .Where(x => x.EntityType == SocialMediaPostEntity.TourTemplate && x.TourTemplateId == entityId && x.Site == SocialMediaSite.Twitter)
                    .ToListAsync(),
                _ => throw new InvalidActionException("INVALID_ACTION_ENTITY_TYPE")
            };

            if (socialMediaPosts.IsNullOrEmpty())
            {
                throw new ResourceNotFoundException($"{entityType.ToString().ToUpper()}_NOT_PUBLISHED");
            }

            List<TweetDTO>? tweetDto = await _redisCacheService.GetAsync<List<TweetDTO>>($"{entityId}-{(int)entityType}");

            tweetDto ??= [];
            foreach (var post in socialMediaPosts)
            {
                var tweet = tweetDto.SingleOrDefault(x => x.XTweetId == post.SocialPostId);
                if (tweet != null)
                {
                    tweet.CreatedAt = post.CreatedAt;
                    tweet.Hashtags = post.SocialMediaPostHashtags.Select(x => x.Hashtag.HashtagName).ToList();
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
                        CreatedAt = post.CreatedAt,
                        Hashtags = post.SocialMediaPostHashtags.Select(x => x.Hashtag.HashtagName).ToList()
                    });
                }
            }

            return tweetDto;
        }

        public async Task PublishPostWithXAsync(string postId, List<string> hashtagName)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            PostTweetRequestDTO postTweetRequestDTO = new()
            {
                Text = $"{post.Title.ToUpper()}\n\nXem thêm tại: https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}\n{string.Join(" ", hashtagName)}",
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
                    PostId = post.PostId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = tweetId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }
                
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishAttractionWithXAsync(string attractionId, List<string> hashtagName)
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
                    Text = $"{attraction.Name.ToUpper()} - Trải nghiệm {attraction.Province.Name} cùng Vietway\n\n📍 {attraction.Address}\n✨ Hãy cùng VietWay khám phá {attraction.Name} tại https://vietway.projectpioneer.id.vn/diem-tham-quan/{attraction.AttractionId}?ref=x\n{string.Join(" ", hashtagName)}",
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
                    AttractionId = attraction.AttractionId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = tweetId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishTourTemplateWithXAsync(string tourTemplateId, List<string> hashtagName)
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
                    Text = $"{tourTemplate.TourName.ToUpper()}\n\n- Thời lượng: {tourTemplate.TourDuration.DurationName}\n- Khởi hành từ: {tourTemplate.Province.Name}\n- Giá từ: {formattedPrice}\n- Đăng ký tại: https://vietway.projectpioneer.id.vn/tour-du-lich/{tourTemplate.TourTemplateId}?ref=x\n{string.Join(" ", hashtagName)}",
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
                    TourTemplateId = tourTemplate.TourTemplateId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = tweetId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishPostToFacebookPageAsync(string postId, List<string> hashtagName)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            string facebookPostId = await _facebookService.PublishPostAsync($"🌟 {post.Title} – Khám Phá Cùng VietWay!\n\n{post.Description}\n\n📢 Đừng bỏ lỡ!\n👉 Tham khảo thêm thông tin du lịch tại:\n\t\t🌐 Website: https://vietway.projectpioneer.id.vn\n\t\t📞 Hotline: 0987 654 321\n\t\t📩 Email: info@vietwaytour.com\n{string.Join(" ", hashtagName)}", $"https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}?ref=facebook");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = facebookPostId,
                    Site = SocialMediaSite.Facebook,
                    EntityType = SocialMediaPostEntity.Post,
                    PostId = post.PostId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = facebookPostId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishAttractionToFacebookPageAsync(string attractionId, List<string> hashtagName)
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
                string facebookPostId = await _facebookService.PublishPostAsync($"{attraction.Name.ToUpper()} - Điểm đến hấp dẫn tại {attraction.Province.Name}\n\n📍 {attraction.Address}\n\nLên kế hoạch cho chuyến đi của bạn ngay hôm nay!\n📸 Đừng quên chụp thật nhiều ảnh và chia sẻ cùng bạn bè nhé!\n👉 Tham khảo thêm thông tin du lịch tại:\n\t\t🌐 Website: https://vietway.projectpioneer.id.vn\n\t\t📞 Hotline: 0987 654 321\n\t\t📩 Email: info@vietwaytour.com\n{string.Join(" ", hashtagName)}", $"https://vietway.projectpioneer.id.vn/diem-tham-quan/{attraction.AttractionId}?ref=facebook");
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = facebookPostId,
                    Site = SocialMediaSite.Facebook,
                    EntityType = SocialMediaPostEntity.Attraction,
                    AttractionId = attraction.AttractionId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = facebookPostId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishTourTemplateToFacebookPageAsync(string tourTemplateId, List<string> hashtagName)
        {
            TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .Include(x => x.Tours)
                .Include(x => x.Province)
                .Include(x => x.TourDuration)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .Include(x => x.Tours)
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

            string startDates = string.Join(", ", tourTemplate.Tours
                .Where(x => x.Status == TourStatus.Opened && ((DateTime)x.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)x.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !x.IsDeleted)
                .Select(y => ((DateTime)y.StartDate).ToString("dd/MM/yyyy")));

            try
            {
                string facebookPostId = await _facebookService.PublishPostAsync($"{tourTemplate.TourName.ToUpper()}\n\n⏰ Thời lượng: {tourTemplate.TourDuration.DurationName}\n🚐 Phương tiện di chuyển: {tourTemplate.Transportation} \n🗺 Khởi hành từ: {tourTemplate.Province.Name}\n📆 Ngày đi: {startDates}\n💵 Giá chỉ từ: {formattedPrice}\n\n Liên hệ tư vấn:\n📞 Hotline: 0987 654 321\n📩 Email: info@vietwaytour.com\n🌐 Website: https://vietway.projectpioneer.id.vn\n\n🔥 Số chỗ có hạn! Đăng ký ngay hôm nay! 🔥\n{string.Join(" ", hashtagName)}", $"https://vietway.projectpioneer.id.vn/tour-du-lich/{tourTemplate.TourTemplateId}?ref=facebook");
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = facebookPostId,
                    Site = SocialMediaSite.Facebook,
                    EntityType = SocialMediaPostEntity.TourTemplate,
                    TourTemplateId = tourTemplate.TourTemplateId,
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);

                foreach (var hashtag in hashtagName)
                {
                    Hashtag? tag = await _unitOfWork.HashtagRepository.Query()
                        .SingleOrDefaultAsync(x => x.HashtagName.Equals(hashtag.ToLower()));
                    if (tag == null)
                    {
                        tag = new Hashtag();
                        tag.HashtagId = _idGenerator.GenerateId();
                        tag.HashtagName = hashtag.Replace("#", "").ToLower();
                        tag.CreatedAt = _timeZoneHelper.GetUTC7Now();
                        await _unitOfWork.HashtagRepository.CreateAsync(tag);
                    }
                    SocialMediaPostHashtag socialMediaPostHashtag = new()
                    {
                        SocialPostId = facebookPostId,
                        HashtagId = tag.HashtagId
                    };
                    await _unitOfWork.SocialMediaPostHashtagRepository.CreateAsync(socialMediaPostHashtag);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<HashtagDTO>> GetHashtags()
        {
            return await _unitOfWork.HashtagRepository.Query()
                .Select(x => new HashtagDTO
                {
                    HashtagId = x.HashtagId,
                    HashtagName = x.HashtagName
                })
                .ToListAsync();
        }
    }
}
