using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.RequestModel
{
    public class HideReviewRequest
    {
        public required bool IsHided { get; set; }
        public string? Reason { get; set; }
    }
}
