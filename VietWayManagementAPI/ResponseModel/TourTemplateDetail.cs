using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.ResponseModel
{
    public class TourTemplateDetail
    {
        public long TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Description { get; set; }
        public required string Duration { get; set; }
        public required int TourCategoryId { get; set; }
        public required string Policy { get; set; }
        public required string Note { get; set; }
        public TourTemplateStatus Status { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required string CreatorName { get; set; }
        public required List<long> Provinces { get; set; }
        public required List<ScheduleDetail> Schedule { get; set; }
        public required List<string> ImageUrls { get; set; }
    }
    public class ScheduleDetail
    {
        public int DayNumber { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<long> Attractions { get; set; }
    }
}
