using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourDetailDTO
    {
        public string TourId { get; set; }
        public string TourTemplateId { get; set; }
        public string StartLocation { get; set; }
        public string? StartLocationPlaceId { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        public int? CurrentParticipant { get; set; }
        public TourStatus Status { get; set; }
        public DateTime? RegisterOpenDate { get; set; }
        public DateTime? RegisterCloseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalBookings { get; set; }
        public decimal? DepositPercent { get; set; }
        public DateTime? PaymentDeadline { get; set; }
        public ICollection<TourPolicyPreviewDTO>? TourPolicies{ get; set; }
        public ICollection<TourPriceDTO>? TourPrices { get; set; }
    }

    public class TourPolicyPreviewDTO
    {
        public DateTime? CancelBefore { get; set; }
        public decimal? RefundPercent { get; set; }
    }

    public class TourPriceDTO
    {
        public string? PriceId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
    }
}
