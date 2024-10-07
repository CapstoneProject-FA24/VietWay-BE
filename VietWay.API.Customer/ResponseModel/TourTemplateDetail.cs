using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.ResponseModel
{
    public class TourTemplateDetail
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        public required DurationDetail Duration { get; set; }
        public required TourCategoryPreview TourCategory { get; set; }
        public required string Policy { get; set; }
        public required string Note { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required List<ProvincePreview> Provinces { get; set; }
        public required List<ScheduleDetail> Schedules { get; set; }
        public required List<ImageDetail> Images { get; set; }
    }
    public class ScheduleDetail
    {
        public int DayNumber { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<AttractionBriefPreview> Attractions { get; set; }
    }
}
