using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class ChangeTourTemplateStatusRequest
    {
        public required TourTemplateStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}
