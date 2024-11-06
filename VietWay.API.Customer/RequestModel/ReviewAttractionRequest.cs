using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Customer.RequestModel
{
    public class ReviewAttractionRequest
    {
        [Range(1,5)]
        public required int Rating { get; set; }
        public string? Review { get; set; }
    }
}
