using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourTemplatePreviewDTO
    {
        public required string TourTemplateId { get; set; }
        public string? Code { get; set; }
        public string? TourName { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public string? TourCategory { get; set; }
        public TourTemplateStatus Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatorName { get; set; }
        public List<string>? Provinces { get; set; }
        public string? ImageUrl { get; set; }
        public string? StartingProvince { get; set; }
    }
}
