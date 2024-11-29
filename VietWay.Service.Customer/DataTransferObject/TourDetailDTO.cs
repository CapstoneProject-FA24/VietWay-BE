using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourDetailDTO
    {
        public required string TourId { get; set; }
        public required string TourTemplateId { get; set; }
        public string? StartLocation { get; set; }
        public string? StartLocationPlaceId { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public DateTime? RegisterOpenDate { get; set; }
        public DateTime? RegisterCloseDate { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        public int CurrentParticipant { get; set; }
        public decimal? DepositPercent { get; set; }

        public required List<TourPriceDTO> PricesByAge { get; set; }
        public required List<TourRefundPolicyDTO> RefundPolicies { get; set; }
    }
}
