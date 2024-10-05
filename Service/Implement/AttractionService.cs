using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.IdHelper;
using static System.Net.Mime.MediaTypeNames;
using Image = VietWay.Repository.EntityModel.Image;

namespace VietWay.Service.Implement
{
    public class AttractionService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IIdGenerator idGenerator) : IAttractionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<string> CreateAttraction(Attraction attraction)
        {
            attraction.AttractionId ??= _idGenerator.GenerateId();
            await _unitOfWork.AttractionRepository.Create(attraction);
            return attraction.AttractionId;
        }

        public async Task DeleteAttraction(Attraction attraction)
        {
            await _unitOfWork.AttractionRepository.SoftDelete(attraction);
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

        public async Task UpdateAttraction(Attraction newAttraction)
        {
            await _unitOfWork.AttractionRepository.Update(newAttraction);
        }
        public async Task UpdateAttractionImage(Attraction attraction, List<IFormFile>? imageFiles, List<string>? removedImageIds)
        {
            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    using Stream stream = imageFile.OpenReadStream();
                    var (publicId, secureUrl) = await _cloudinaryService.UploadImageAsync(stream, imageFile.FileName);
                    Image image = new()
                    {
                        ImageId = _idGenerator.GenerateId(),
                        PublicId = publicId,
                        Url = secureUrl,
                        ContentType = imageFile.ContentType,
                        FileName = imageFile.FileName
                    };
                    AttractionImage attractionImage = new()
                    {
                        AttractionId = attraction.AttractionId,
                        ImageId = image.ImageId,
                        Image = image
                    };
                    attraction.AttractionImages.Add(attractionImage);
                }
            }
            if (removedImageIds?.Count > 0)
            {
                var removedImages = attraction.AttractionImages
                    .Where(x => removedImageIds.Contains(x.ImageId))
                    .ToList();
                foreach (var image in removedImages)
                {
                    attraction.AttractionImages.Remove(image);
                }
                _ = await _cloudinaryService.DeleteImages(removedImages.Select(x => x.Image.PublicId));
                await _unitOfWork.AttractionRepository.Update(attraction);
                await _unitOfWork.ImageRepository.DeleteRange(removedImages.Select(x => x.Image));
            }
        }
    }
}
