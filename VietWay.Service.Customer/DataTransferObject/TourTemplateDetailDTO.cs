using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourTemplateDetailDTO
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        //public required TourDurationPreviewDTO Duration { get; set; }
        //public required TourCategoryDTO TourCategory { get; set; }
        public required string Policy { get; set; }
        public required string Note { get; set; }
        public TourTemplateStatus Status { get; set; }
        //public required List<ProvincePreviewDTO> Provinces { get; set; }
        //public required List<ScheduleDTO> Schedules { get; set; }
        //public required List<ImageDetail> Images { get; set; }
    }
}
