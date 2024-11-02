using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class AttractionService(IUnitOfWork unitOfWork) : IAttractionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<AttractionDetailDTO?> GetAttractionDetailByIdAsync(string attractionId)
        {
            return await _unitOfWork.AttractionRepository
                .Query()
                .Where(x => x.AttractionId.Equals(attractionId))
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
                        Name = x.Province.Name,
                    },
                    Images = x.AttractionImages.Select(y => new ImageDTO
                    {
                        ImageId = y.ImageId,
                        Url = y.ImageUrl
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<(int count, List<AttractionPreviewDTO>)> GetAttractionsPreviewAsync(string? nameSearch, List<string>? provinceIds, 
            List<string>? attractionTypeIds, int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query();
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
                .Skip(pageSize * (pageIndex-1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.Name,
                    AttractionCategory = x.AttractionCategory.Name,
                    ImageUrl = x.AttractionImages.FirstOrDefault().ImageUrl
                })
                .ToListAsync();
            return (count, items);
        }
    }
}
