using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourTemplateDetailDTO
    {
        public required string TourTemplateId { get; set; }
        public string? Code { get; set; }
        public string? TourName { get; set; }
        public string? Description { get; set; }
        public required TourDurationDTO Duration { get; set; }
        public required TourCategoryDTO TourCategory { get; set; }
        public string? Note { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public ProvinceBriefPreviewDTO? StartingProvince { get; set; }
        public string? Transportation { get; set; }
        public List<ProvinceBriefPreviewDTO>? Provinces { get; set; }
        public List<ScheduleDTO>? Schedules { get; set; }
        public List<ImageDTO>? Images { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
