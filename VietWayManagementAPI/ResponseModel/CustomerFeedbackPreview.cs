namespace VietWay.API.Management.ResponseModel
{
    public class CustomerFeedbackPreview
    {
        public required string FeedbackId { get; set; }
        public required string BookingId { get; set; }
        public required int Rating { get; set; }
        public required string Feedback { get; set; }
    }
}
