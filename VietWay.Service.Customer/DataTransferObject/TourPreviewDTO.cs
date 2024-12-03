using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourPreviewDTO
    {
        public required string TourId { get; set; }
        public string? StartLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public int? MaxParticipant { get; set; }
        public int CurrentParticipant { get; set; }
        public decimal? DepositPercent { get; set; }
        public DateTime? PaymentDeadline { get; set; }
        public required List<TourRefundPolicyDTO> RefundPolicies { get; set; }
    }
}
