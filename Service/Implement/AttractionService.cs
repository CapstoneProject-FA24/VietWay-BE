using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.IdHelper;
using Image = VietWay.Repository.EntityModel.Image;

namespace VietWay.Service.Implement
{
    public class AttractionService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IIdGenerator idGenerator) : IAttractionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task CreateAttraction(Attraction attraction, List<IFormFile> imageFiles, bool isDraft)
        {
            attraction.AttractionId ??= _idGenerator.GenerateId();
            attraction.Status = isDraft ? AttractionStatus.Draft : AttractionStatus.Pending;
            attraction.AttractionImages = [];
            foreach (IFormFile imageFile in imageFiles)
            {
                using Stream imageStream = imageFile.OpenReadStream();
                (string publicId, string secureUrl) = await _cloudinaryService.UploadImageAsync(imageStream, imageFile.FileName);
                Image image = new()
                {
                    PublicId = publicId,
                    Url = secureUrl,
                    ContentType = imageFile.ContentType,
                    FileName = imageFile.FileName,
                    ImageId = _idGenerator.GenerateId()
                };
                attraction.AttractionImages.Add(new AttractionImage
                {
                    Image = image,
                    AttractionId = attraction.AttractionId,
                    ImageId = image.ImageId
                });
            }
            await _unitOfWork.AttractionRepository.Create(attraction);
        }

        public Task DeleteAttraction(string attractionId)
        {
            throw new NotImplementedException();
        }

        public async Task<(int totalCount, List<Attraction> items)> GetAllAttractions(
            string? nameSearch, 
            List<string>? provinceIds, 
            List<string>? attractionTypeIds, 
            AttractionStatus? status,
            int pageSize, 
            int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query();
            if (false == string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (null != provinceIds && provinceIds.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (null != attractionTypeIds && attractionTypeIds.Count > 0)
            {
                query = query.Where(x => attractionTypeIds.Contains(x.AttractionTypeId));
            }
            if (null != status)
            {
                query = query.Where(x => x.Status == status);
            }
            int count = await query.CountAsync();
            List<Attraction> attractions = await query
                .Include(x => x.AttractionImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.Province)
                .Include(x => x.AttractionType)
                .Include(x=>x.Creator)
                .Skip(pageSize * (pageIndex-1))
                .Take(pageSize)
                .ToListAsync();
            return (count, attractions);
        }

        public async Task<Attraction?> GetAttractionById(string attractionId)
        {
            return await _unitOfWork.AttractionRepository
                .Query()
                .Where(x => x.AttractionId.Equals(attractionId))
                .Include(x => x.AttractionImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.Province)
                .Include(x => x.AttractionType)
                .Include(x=>x.Creator)
                .SingleOrDefaultAsync();
        }

        public Task UpdateAttraction(Attraction attraction, List<IFormFile> images)
        {
            throw new NotImplementedException();
        }
    }
}
