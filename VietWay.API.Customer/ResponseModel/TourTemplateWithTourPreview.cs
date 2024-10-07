using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.ResponseModel
{
    public class TourTemplateWithTourPreview
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required List<string> Provinces { get; set; }
        public required string ImageUrl { get; set; }
        public required List<decimal> Price { get; set; }
        public required List<DateTime> StartDate { get; set; }
        public required List<DateTime> EndDate { get; set; }
    }
}
