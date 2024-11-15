namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourTemplatePreviewDTO
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public required List<string> Provinces { get; set; }
        public required decimal Price { get; set; } 
        public required string ImageUrl { get; set; }
    }
}
