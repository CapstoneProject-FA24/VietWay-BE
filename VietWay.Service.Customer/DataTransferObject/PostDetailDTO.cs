namespace VietWay.Service.Customer.DataTransferObject
{
    public class PostDetailDTO
    {
        public required string PostId { get; set; }
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public required string Content { get; set; }
        public required string PostCategoryId { get; set; }
        public required string PostCategoryName { get; set; }
        public required string ProvinceId { get; set; }
        public required string ProvinceName { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public bool IsLiked { get; set; }
    }
}
