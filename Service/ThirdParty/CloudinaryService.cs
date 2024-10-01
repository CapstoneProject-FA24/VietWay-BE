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
            _cloudinary = new("cloudinary://733166211479731:-JO2RA4VVnLtYOFCDdbNEHx-etI@djfopoob1");
            _cloudinary.Api.Secure = true;
        }
        public string GetImage(string publicId)
        {
            return _cloudinary.Api.UrlImgUp.BuildUrl();
        }
        public async Task<string> UploadImageAsync(Stream stream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return JsonSerializer.Serialize(uploadResult);
        }
        public async Task<string> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
            return JsonSerializer.Serialize(deleteResult);
        }

    }
}
