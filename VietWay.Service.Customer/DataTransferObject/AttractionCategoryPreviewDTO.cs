namespace VietWay.Service.Customer.DataTransferObject
{
    public class AttractionCategoryPreviewDTO
    {
        public required string AttractionCategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}