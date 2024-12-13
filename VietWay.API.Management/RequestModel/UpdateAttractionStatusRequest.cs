using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class UpdateAttractionStatusRequest
    {
        public required AttractionStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
