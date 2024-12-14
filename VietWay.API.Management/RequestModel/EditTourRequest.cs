using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.RequestModel
{
    public class EditTourRequest
    {
        public string? StartLocation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Default tourist price must be positive.")]
        public decimal? DefaultTouristPrice { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? RegisterOpenDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? RegisterCloseDate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Min participants must be non-negative.")]
        public int? MinParticipant { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Max participants must be non-negative.")]
        public int? MaxParticipant { get; set; }
        public List<TourPrice>? TourPrice {  get; set; }
        public List<RefundPolicy>? RefundPolicies { get; set; }
    }
    public class TourPrice
    {
        public string Name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "AgeFrom must be non-negative.")]
        public int AgeFrom { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "AgeTo must be non-negative.")]
        public int AgeTo { get; set; }
    }
    public class RefundPolicyInfo
    {
        [DataType(DataType.DateTime)]
        public DateTime CancelBefore { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "RefundPercent must be positive.")]
        public decimal RefundPercent { get; set; }
    }
}
