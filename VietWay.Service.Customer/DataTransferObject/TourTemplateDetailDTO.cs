namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourTemplateDetailDTO
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        public required TourDurationDTO Duration { get; set; }
        public required TourCategoryDTO TourCategory { get; set; }
        public required string Note { get; set; }
        public required List<ProvincePreviewDTO> Provinces { get; set; }
        public required List<ScheduleDTO> Schedules { get; set; }
        public required List<ImageDTO> Images { get; set; }
    }
}
