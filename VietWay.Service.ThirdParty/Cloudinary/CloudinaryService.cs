using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CloudinaryClient = CloudinaryDotNet.Cloudinary;
namespace VietWay.Service.ThirdParty.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryClient _cloudinary;
        public CloudinaryService(CloudinaryApiConfig config)
        {
            _cloudinary = new($"cloudinary://{config.ApiKey}:{config.ApiSecret}@{config.CloudName}");
            _cloudinary.Api.Secure = true;
        }
        public string GetImage(string publicId)
        {
            return _cloudinary.Api.UrlImgUp
                .Transform(new Transformation()
                    .Width(1000).Crop("scale").Chain()
                    .Quality("auto").Chain()
                    .FetchFormat("auto"))
                .BuildUrl(publicId);
        }
        public async Task UploadImageAsync(string publicId, string fileName, byte[] fileStream)
        {
            using MemoryStream stream = new(fileStream);
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(fileName,stream),
                PublicId = publicId
            };
            _ = await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task DeleteImagesAsync(IEnumerable<string> publicIds)
        {
            DelResParams deleteParams = new()
            {
                PublicIds = publicIds.ToList()
            };
            _ = await _cloudinary.DeleteResourcesAsync(deleteParams);
        }
    }
}
