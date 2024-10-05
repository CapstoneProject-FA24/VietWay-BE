using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService()
        {
            //string cloudinaryConnectionString = Environment.GetEnvironmentVariable("Cloudinary_ConnectionString") ??
            //throw new Exception("Can not get Cloudinary connection string");
            _cloudinary = new("cloudinary://733166211479731:-JO2RA4VVnLtYOFCDdbNEHx-etI@djfopoob1");
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
        public async Task<(string publicId, string secureUrl)> UploadImageAsync(Stream stream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return (uploadResult.PublicId, uploadResult.SecureUrl.ToString());
        }

        public async Task<bool> DeleteImages(IEnumerable<string> images)
        {
            DelResParams deleteParams = new()
            {
                PublicIds = images.ToList()
            };
            DelResResult result = await _cloudinary.DeleteResourcesAsync(deleteParams);
            return result.Deleted.Count == images.Count();
        }
    }
}
