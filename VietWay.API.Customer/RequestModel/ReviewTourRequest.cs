namespace VietWay.API.Customer.RequestModel
{
    public class ReviewTourRequest
    {
        public required int Rating { get; set; }
        public string? Content { get; set; }
        public required bool IsPublic { get; set; } 
    }
}
