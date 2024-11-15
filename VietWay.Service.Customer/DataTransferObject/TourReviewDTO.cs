using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourReviewDTO
    {
        public required string ReviewId { get; set; } = default!;
        public required int Rating { get; set; } = default!;
        public string? Review { get; set; } = default;
        public required DateTime CreatedAt { get; set; } = default;
        public string? Reviewer { get; set; }
    }
}
