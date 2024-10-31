using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CloudinaryClient = CloudinaryDotNet.Cloudinary;
namespace VietWay.Service.ThirdParty.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryClient _cloudinary;
        public CloudinaryService()
        {
            string apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")
                ?? throw new Exception("CLOUDINARY_API_KEY is not set in environment variables");
            string apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                ?? throw new Exception("CLOUDINARY_API_SECRET is not set in environment variables");
            string cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")
                ?? throw new Exception("CLOUDINARY_CLOUD_NAME is not set in environment variables");
            _cloudinary = new($"cloudinary://{apiKey}:{apiSecret}@{cloudName}");
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
        public async Task UploadImageAsync(string publicId, string filePath, string fileName)
        {
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(fileName, filePath),
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
