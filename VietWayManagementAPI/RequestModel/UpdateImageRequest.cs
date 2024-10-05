namespace VietWay.API.Management.RequestModel
{
    public class UpdateImageRequest
    {
        public List<IFormFile>? NewImages { get; set; }
        public List<string>? DeletedImageIds { get; set; }
    }
}
