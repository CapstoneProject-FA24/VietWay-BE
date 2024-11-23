using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class ChangePostStatusRequest
    {
        public required PostStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
