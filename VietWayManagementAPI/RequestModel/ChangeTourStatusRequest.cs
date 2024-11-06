using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class ChangeTourStatusRequest
    {
        public required TourStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
