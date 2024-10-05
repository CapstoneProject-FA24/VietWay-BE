using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.ResponseModel
{
    public class AttractionSchedulePreview
    {
        public required string TourTemplateId { get; set; }
        public required int DayNumber { get; set; }
        public required string AttractionId { get; set; }
    }
}
