using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.ResponseModel
{
    public class TourTemplatePreview
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required string CreatorName { get; set; }
        public required List<string> Provinces { get; set; }
        public required string ImageUrl { get; set; }
    }
}
