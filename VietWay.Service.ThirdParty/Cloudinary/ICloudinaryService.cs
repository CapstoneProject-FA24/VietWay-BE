﻿namespace VietWay.Service.ThirdParty.Cloudinary
{
    public interface ICloudinaryService
    {
        public string GetImage(string publicId);
        public Task UploadImageAsync(string publicId, string filePath, string fileName);
        public Task DeleteImagesAsync(IEnumerable<string> imageIds);
    }
}
