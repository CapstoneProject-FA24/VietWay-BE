namespace VietWay.Service.Management.DataTransferObject
{
    public class AttractionReviewDTO
    {
        public required string ReviewId { get; set; } = default!;
        public required int Rating { get; set; } = default!;
        public string? Review { get; set; } = default;
        public required DateTime CreatedAt { get; set; } = default;
        public required string Reviewer { get; set; } = default!;
        public required int LikeCount { get; set; } = default!;
        public required bool IsDeleted { get; set; } = default!;
    }
}
