using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourTemplateWithTourInfoDTO
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public required string Description { get; set; }
        public required string Note { get; set; }
        public required string StartingProvince { get; set; }
        public required List<string> Provinces { get; set; }
        public required string ImageUrl { get; set; }
        public required List<ScheduleDTO> Schedules { get; set; }
        public List<TourInfoDTO> Tours { get; set; }
    }
    public class TourInfoDTO
    {
        public required string TourId { get; set; }
        public string StartLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        public int? CurrentParticipant { get; set; }
        public List<TourPriceDTO> TourPrices { get; set; }
        public List<TourPolicyPreviewDTO> TourPolicies { get; set; }
    }
}
