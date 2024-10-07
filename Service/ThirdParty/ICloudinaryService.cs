namespace VietWay.Service.ThirdParty
{
    public interface ICloudinaryService
    {
        public string GetImage(string publicId);
        public Task<(string publicId, string secureUrl)> UploadImageAsync(Stream stream, string fileName);
        public Task<bool> DeleteImages(IEnumerable<string> images);
    }
}
