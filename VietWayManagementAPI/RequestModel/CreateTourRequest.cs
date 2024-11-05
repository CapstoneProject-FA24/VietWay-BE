using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourRequest
    {
        public required string TourTemplateId { get; set; }
        public string? StartLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public DateTime? RegisterOpenDate { get; set; }
        public DateTime? RegisterCloseDate { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        public virtual ICollection<TourPriceInfo>? TourPrices { get; set; }
        public virtual ICollection<RefundPolicy>? RefundPolicies { get; set; }
    }
    public class TourPriceInfo
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int AgeFrom { get; set; }
        public required int AgeTo { get; set; }
    }
    public class RefundPolicy
    {
        public required DateTime CancelBefore { get; set; }
        public required decimal RefundPercent { get; set; }
    }
}
