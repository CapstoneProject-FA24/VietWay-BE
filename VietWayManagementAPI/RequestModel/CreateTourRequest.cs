using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourRequest
    {
        public required string TourTemplateId { get; set; }
        public required string StartLocation { get; set; }
        public required DateTime StartDate { get; set; }
        public required decimal DefaultTouristPrice { get; set; }
        public required DateTime RegisterOpenDate { get; set; }
        public required DateTime RegisterCloseDate { get; set; }
        public required int MaxParticipant { get; set; }
        public required int MinParticipant { get; set; }
        public required decimal DepositPercent { get; set; }
        public required string StartLocationPlaceId { get; set; }
        public List<TourPrice>? TourPrice { get; set; }
        public List<RefundPolicy>? RefundPolicies { get; set; }
        public required DateTime PaymentDeadline { get; set; }
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
