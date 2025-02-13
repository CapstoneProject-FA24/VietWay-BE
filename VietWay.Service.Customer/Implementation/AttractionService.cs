﻿using IdGen;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Customer.Implementation
{
    public class AttractionService(IUnitOfWork unitOfWork, IRedisCacheService redisCacheService) : IAttractionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task<AttractionDetailDTO?> GetAttractionDetailByIdAsync(string attractionId, string? customerId, SocialMediaSite? socialMediaSite)
        {
            AttractionDetailDTO attraction = await _unitOfWork.AttractionRepository
                .Query()
                .Where(x => x.AttractionId.Equals(attractionId) && x.Status == AttractionStatus.Approved && x.IsDeleted == false)
                .Select(x => new AttractionDetailDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Description = x.Description,
                    Address = x.Address,
                    ContactInfo = x.ContactInfo,
                    GooglePlaceId = x.GooglePlaceId,
                    Website = x.Website,
                    AttractionCategory = new AttractionCategoryPreviewDTO
                    {
                        AttractionCategoryId = x.AttractionCategory.AttractionCategoryId,
                        Name = x.AttractionCategory.Name,
                        Description = x.AttractionCategory.Description
                    },
                    Province = new ProvincePreviewDTO
                    {
                        ProvinceId = x.Province.ProvinceId,
                        Description = x.Province.Description,
                        Name = x.Province.Name,
                    },
                    Images = x.AttractionImages.Select(y => new ImageDTO
                    {
                        ImageId = y.ImageId,
                        Url = y.ImageUrl
                    }).ToList(),
                    AverageRating = x.AttractionReviews.Where(x => false == x.IsDeleted).Average(y => y.Rating),
                    RatingCount = x.AttractionReviews.Where(x => false == x.IsDeleted)
                        .GroupBy(g => g.Rating)
                        .Select(s => new RatingDTO { Rating = s.Key, Count = s.Count() })
                        .ToList(),
                    LikeCount = x.AttractionLikes.Count(),
                    IsLiked = customerId != null && x.AttractionLikes.Any(x => x.CustomerId.Equals(customerId))
                })
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXISTED_ATTRACTION");
            
            switch (socialMediaSite)
            {
                case SocialMediaSite.Facebook:
                    await _redisCacheService.IncrementAsync($"facebookReferrence-{SocialMediaPostEntity.Attraction}-{attractionId}");
                    break;
                case SocialMediaSite.Twitter:
                    await _redisCacheService.IncrementAsync($"twitterReferrence-{SocialMediaPostEntity.Attraction}-{attractionId}");
                    break;
                default:
                    await _redisCacheService.IncrementAsync($"siteReferrence-{SocialMediaPostEntity.Attraction}-{attractionId}");
                    break;
            }

            return attraction;
        }

        public async Task<PaginatedList<AttractionPreviewDTO>> GetAttractionsPreviewAsync(string? nameSearch, List<string>? provinceIds, 
            List<string>? attractionTypeIds, string? customerId , int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query()
                .Where(x => x.Status == AttractionStatus.Approved && x.IsDeleted == false);
            if (nameSearch != null)
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (attractionTypeIds?.Count > 0)
            {
                query = query.Where(x => attractionTypeIds.Contains(x.AttractionCategoryId));
            }
            int count = await query.CountAsync();
            List<AttractionPreviewDTO> items = await query
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.Name,
                    AttractionCategory = x.AttractionCategory.Name,
                    ImageUrl = x.AttractionImages.Select(x => x.ImageUrl).First(),
                    AverageRating = x.AttractionReviews.Where(x => false == x.IsDeleted).Average(y => y.Rating),
                    LikeCount = x.AttractionLikes.Count(),
                    IsLiked = customerId != null && x.AttractionLikes.Any(x => x.CustomerId.Equals(customerId))
                })
                .ToListAsync();
            return new PaginatedList<AttractionPreviewDTO>
            {
                Total = count,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = items
            };
        }

        public async Task<PaginatedList<AttractionPreviewDTO>> GetCustomerLikedAttractionsAsync(string customerId, int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query()
                .Where(x => x.AttractionLikes.Any(x => x.CustomerId.Equals(customerId)));
            int count = query.Count();
            List<AttractionPreviewDTO> items = await query
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.Name,
                    AttractionCategory = x.AttractionCategory.Name,
                    ImageUrl = x.AttractionImages.Select(x => x.ImageUrl).First(),
                    AverageRating = x.AttractionReviews.Where(x => false == x.IsDeleted).Average(y => y.Rating),
                    LikeCount = x.AttractionLikes.Count(),
                    IsLiked = true
                })
                .ToListAsync();
            return new PaginatedList<AttractionPreviewDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }

        public async Task ToggleAttractionLikeAsync(string attractionId, string customerId, bool isLike)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                AttractionLike? attractionLike = await _unitOfWork.AttractionLikeRepository.Query()
                    .SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId) && x.CustomerId.Equals(customerId));
                if (null == attractionLike && isLike)
                {
                    await _unitOfWork.AttractionLikeRepository.CreateAsync(new AttractionLike
                    {
                        CustomerId = customerId,
                        AttractionId = attractionId,
                        CreatedAt = DateTime.Now
                    });
                }
                else if (null != attractionLike && false == isLike)
                {
                    await _unitOfWork.AttractionLikeRepository.DeleteAsync(attractionLike);
                }
                else
                {
                    throw new InvalidActionException("INVALID_ACTION_ATTRACTION_LIKE");
                }
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
